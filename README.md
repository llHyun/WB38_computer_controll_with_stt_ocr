🎙️ 음성 인식 기반 컴퓨터 제어 시스템
Voice Recognition Computer Control System

📝 프로젝트 소개 (Overview)
본 프로젝트는 STT(Speech-to-Text) 기술과 OCR(Optical Character Recognition) 기술을 결합하여, 하드웨어(키보드, 마우스) 없이 사용자의 목소리만으로 컴퓨터를 제어할 수 있는 시스템입니다.

사용자의 편의를 위해 음성 명령어를 단축키로 매핑할 수 있으며, 보안과 오작동 방지를 위해 LSTM 기반의 화자 인식 기능을 도입하여 사전에 등록된 사용자만 제어할 수 있도록 설계되었습니다.

🎯 설계 주안점

핸즈프리 제어: STT를 활용한 텍스트 추출 및 컴퓨터 제어 커맨드 실행

시각 정보 인식: 화면 캡처 및 OCR을 통한 텍스트 좌표 추출로 마우스 이벤트 제어

개인화 & 보안: 오디오 데시벨(dB) 트리거 및 딥러닝 기반 화자 인식(Speaker Verification)

사용자 편의성: 자주 사용하는 명령어를 저장하고 백그라운드에서 상시 대기

✨ 주요 기능 (Key Features)
1. 🗣️ STT (Speech-to-Text)

자동 녹음 트리거: 마이크 입력이 일정 데시벨(dB) 이상이 될 때 자동으로 녹음을 시작합니다.

음성 텍스트 변환: 녹음된 오디오 파일을 서버의 STT 엔진(Azure Speech API)으로 전송하여 텍스트 명령어를 추출합니다.

단축키 매핑: 특정 음성 명령어를 사용자가 설정한 키보드 단축키로 변환하여 실행합니다.

2. 👁️ OCR (Optical Character Recognition)

화면 텍스트 인식: 사용자가 '선택'이라는 명령어를 말하면 현재 화면을 캡처합니다.

좌표 추출 및 제어: Google Cloud Vision API를 통해 화면 내 텍스트의 좌표를 추출하고, 해당 위치에 마우스 클릭 이벤트를 발생시킵니다.

3. 🔐 화자 인식 (Speaker Recognition)

특징 추출: 입력된 음성 데이터에서 MFCC(Mel-frequency cepstral coefficients) 특징을 추출합니다.

딥러닝 모델: 추출된 데이터를 LSTM(Long Short-Term Memory) 모델에 학습시켜, 등록된 화자의 목소리인지 판별합니다.


🛠️ 기술 스택 (Tech Stack)
- 개발 환경: Windows 11
- 개발 언어: C# / Phython 3.11.5 / JavaScript
- 개발 도구: Visual Studio 2022 / Visual Studio Code / Jupyter Notebook / MySQL
- 라이브러리 : Tensorflow / librosa / google.cloud.vision_v1(Google OCR api)
               azure.cognitiveservices.speech(AZURE STT api) / Node.js


🚀 기대 효과
접근성 향상: 신체적 제약 등으로 키보드/마우스 사용이 어려운 사용자에게 컴퓨터 접근 기회를 제공합니다.

편의성 증대: 업무, 여가, 일상생활 등 다양한 환경에서 하드웨어 조작 없이 능동적이고 효율적인 컴퓨터 제어가 가능합니다.


👥 팀원 (Team)

팀장:	이도현

팀원:	노가연, 박도연, 유진수, 조용호

<img width="905" height="465" alt="image" src="https://github.com/user-attachments/assets/870ed7aa-c52b-4ab1-a139-9faa62cc988d" />
<img width="923" height="688" alt="image" src="https://github.com/user-attachments/assets/2c285aef-fbd8-4684-a4ef-9870ad1da025" />

