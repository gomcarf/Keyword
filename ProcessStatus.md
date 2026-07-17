# PCB - Process Status

PCB(Process Control Block, 프로세스 제어 블록)에서 프로세스 상태(Process Status / Process State)는 운영체제가 CPU 스케줄러를 통해 프로세스들을 효율적으로 관리하기 위해 기록하는 정보. 프로세스는 생성되어 소멸할 때까지 끊임없이 상태가 변하며, PCB는 이 변화하는 상태를 실시간으로 기록.

## 1. 프로세스 상태(5-State Model)

```markdown
	    [Admitted]           [Dispatch]
(New) -------------> (Ready) ----------> (Running) -----------> (Terminated)
						^                    |      [Exit]
						|    [Timeout]       |
						+--------------------+
						|                    |
						|    [I/O or Event]  |
						+--------------------+
							 		|
									v
								(Waiting)
```

### ① 생성 (New)

- **정의 :** 프로세스가 이제 막 메모리에 올라와 PCB를 할당받은 상태.
- **구체적 의미 :** OS가 프로세스를 생성하는 중이며, 아직 CPU를 사용할 준비가 완벽히 끝나지 않음. 메모리 공간 확보 및 자원 할당이 완료되면 `Ready` 상태로 전이(Admitted)됨.

### ② 준비 (Ready)

- **정의 :** CPU를 할당받기 위해 대기하는 상태.
- **구체적 의미 :** 실행에 필요한 메모리, 입출력 장치 등의 모든 자원을 할당받은 상태에서 오직 CPU(프로세서) 제어권만을 기다리고 있는 상태.
- **관리 방식 :** CPU 스케줄러가 관리하는 준비 큐(Ready Queue)에서 대기.

### ③ 실행 (Running)

- **정의 :** 실제로 CPU를 차지하고 기계어 명령어를 실행하고 있는 상태.
- **구체적 의미 :** CPU 스케줄러에 의해 선택되어(Dispatch), 프로세스의 코드가 CPU 내의 레지스터(PC 등)를 사용해 실행 중인 상태.
- **전이 경로 :**
    - 할당된 시간(Time Slice)을 모두 소모하면 타이머 인터럽트에 의해 다시 `Ready`로 돌아감 (Timeout).
    - 실행 중 입출력(I/O) 요청이나 이벤트가 발생하면 `Waiting` 상태로 내려감.
    - 실행이 완료되면 `Terminated` 상태가 됨.

### ④ 대기 / 보류 (Waiting / Blocked)

- **정의 :** 입출력(I/O) 완료, 디스크 읽기, 네트워킹 데이터 수신 등 특정 이벤트가 발생할 때까지 기다리는 상태.
- **구체적 의미 :** 이 상태의 프로세스는 CPU를 주고 싶어도 당장 일을 처리할 수 없음(예: 키보드 입력 대기 중).
- **전이 경로 :** 기다리던 이벤트나 입출력이 완료(I/O Completion)되면 CPU가 바로 실행하는 것이 아니라, 다시 **`Ready`** 상태로 돌아가 자기 차례를 기다려야 함.

### ⑤ 종료 (Terminated / Destroy)

- **정의 :** 프로세스 실행이 완료되어 자원을 반납하는 상태.
- **구체적 의미 :** 프로세스가 사용하던 메모리 영역(Code, Data, Stack, Heap)은 모두 회수됨. 단, 운영체제가 프로세스의 종료 상태(Exit Code)를 확인하고 PCB를 완전히 제거하기 전까지 아주 짧은 순간 동안 PCB의 상태가 `Terminated`로 유지된다.

## 2. 보류 상태 (Suspended State)

**스와핑(Swapping)** : 컴퓨터의 물리 메모리(RAM)가 꽉 차면, OS는 프로세스 중 일부를 통째로 디스크의 스왑 영역(Swap Area)으로 보냄.

| **상태** | **설명** |
| --- | --- |
| **Suspended Ready**<br>(준비 보류) | Ready 상태에서 메모리가 부족해 디스크로 쫓겨난(Swap-out) 상태.<br>메모리가 확보되면 다시 Ready 상태로 돌아옴(Swap-in). |
| **Suspended Blocked**<br>(대기 보류) | Waiting 상태에서 메모리가 부족해 디스크로 쫓겨난 상태. <br>대기하던 이벤트가 완료되면 `Suspended Ready` 상태로 전이. |

## 역할

PCB는 구조체 형태로 이 상태 값을 저장하고 관리

```c
struct task_struct {
volatile long state;    /* -1: unrunnable, 0: runnable, >0: stopped */
void *stack;
unsigned int flags;
// ... 기타 프로세스 제어 정보들
}
```

- **문맥 교환(Context Switch)의 기준 :** CPU 스케줄러는 PCB 내부의 `state` 필드를 수시로 확인하여 현재 어떤 프로세스를 실행할지 결정. `state`가 `Ready(0)`가 아닌 프로세스는 절대로 CPU를 할당받을 수 없다.
- **상태 전이의 동기화 :** 프로세스 상태가 변할 때마다 OS는 PCB의 상태 필드를 즉시 갱신하고, 해당 프로세스가 대기해야 하는 큐(Queue)의 링크드 리스트 위치를 이동시킴.
