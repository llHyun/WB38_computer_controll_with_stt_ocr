const express = require('express');
const fs = require('fs');
const spawn = require('child_process').spawn;
const multer = require('multer');
const path = require('path');
const upload = multer();    // for parsing multipart/form-data
const router = express.Router();
const { Console, error } = require('console');
const localimgpath = "C:\\Temp\\Server"
const chokidar = require('chokidar');

createFile(localimgpath)

//클라이언트로부터 WAV파일과 ID, image파일을 받아 이미지는 미리 저장하고(속도 증강을 위해)
//WAV 파일을 STT API에게 전송해 나온 결과값을 클라이언트에게 전송
//위 과정 이전에 (모델이 WAV 파일이 들어오면 검증을 시작 -> 결과값을 TXT로 저장)
// 해당 /tell post는 TXT 변화를 감지 -> 결과값 읽어오기(아직 구현 안함) -> 결과값을 토대로 클라이언트에게 전송
router.post('/tell', upload.fields([{name: 'id', maxCount: 1}, {name: 'image', maxCount: 1}, {name: 'record', maxCount: 1}]), async(req,res)=>{
    let imageByteArray = req.files.image[0].buffer;
    let wavfile = req.files.record[0].buffer;
    let id = req.body.id;
    //TXT파일 변경 감지를 위한 경로 지정 + 변화감지 할 수 있는 코드 (비동기)
    const directoryPath = localimgpath + '/' + id + '/result';
    async function watchForModel() {
        return new Promise((resolve, reject) => {
            const watcher = chokidar.watch(directoryPath).on('change', (path) => {
                watcher.close(); // 변경 감지를 중단
                console.log('Change detected.');
                resolve();
            });
    
            // 8초 후에도 파일 변경이 감지되지 않으면 에러 반환
            setTimeout(() => {
                watcher.close();
                reject(new Error('Timeout: 모델이 값을 주지 않음'));
            }, 8000); // 타임아웃 시간 설정
        });
    }

    //WAV 파일 저장
    let wavPath = localimgpath + '/' + id + '/record/lastrec.wav';  // 경로 및 파일명 수정
    fs.writeFileSync(wavPath, Buffer.from(wavfile));
    //이미지 저장
    let imagePath = localimgpath +'/' + id + '/screenshot.jpg';
    fs.writeFileSync(imagePath, Buffer.from(imageByteArray));

    try {
        //TXT파일에 모델이 값을 줄 때 까지 대기
        await watchForModel();
    } catch (error) {
        console.log(error.message);
        // 에러에 따른 적절한 응답을 클라이언트에게 전송
        res.json({
            nomodel: "모델이 값을 주지 않음"
        })
        return;
    }

    //여기에 TXT 파일 읽기
    fs.readFile(directoryPath + "/result.txt", 'utf8', (err, data) => {
        if (err) {
            res.json({
                error: "파일 읽기 실패"
            })
          return;
        }
        if(!data) {
            res.json({
                error: "파일에 데이터가 없습니다."
            })
            console.log("파일에 데이터가 없습니다.")
            return;
        }
        if(data === "false"){
            res.json({
                nospeak: "화자 아님"
            })
            console.log("화자 아님")
            return
        }
        else{
            resultStt(id, localimgpath, res);
        }
    });
})

router.post('/tellnomodel', upload.fields([{name: 'id', maxCount: 1}, {name: 'image', maxCount: 1}, {name: 'record', maxCount: 1}]), async(req,res)=>{
    let imageByteArray = req.files.image[0].buffer;
    let wavfile = req.files.record[0].buffer;
    let id = req.body.id;
    //TXT파일 변경 감지를 위한 경로 지정 + 변화감지 할 수 있는 코드 (비동기)
    

    //WAV 파일 저장
    let wavPath = localimgpath + '/' + id + '/record/lastrec.wav';  // 경로 및 파일명 수정
    fs.writeFileSync(wavPath, Buffer.from(wavfile));
    //이미지 저장
    let imagePath = localimgpath +'/' + id + '/screenshot.jpg';
    fs.writeFileSync(imagePath, Buffer.from(imageByteArray));
    resultStt(id, localimgpath, res);
})

async function resultStt(id, localimgpath, res){
    //STT 결과값에 따라서 사용자에게 정보 전달4
    try{
        const stt_result = await runSTT(id, localimgpath);
            console.log("stt결과값 : " + stt_result);
            if(stt_result === null || stt_result.trim() === ''){
                res.json({
                    empty: "공백"
                })
            }else{
                res.json({
                    Stt_Result: stt_result
                })
            }
    }
    catch(error){
        res.json({
            error: error
        })
    }
}

//OCR에서 중복처리가 되어 라벨이 띄워졌을때 클라이언트에게서 오는 요청
//동일하게 WAV파일을 받아 STT 처리를 하고 클라이언트한테 돌려줌
router.post('/ttn', upload.fields([{name: 'id', maxCount: 1}, {name: 'record', maxCount: 1}]), async(req,res)=>{
    try {
        let wavfile = req.files.record[0].buffer;
        let id = req.body.id;
        let wavPath = localimgpath + '/' + id + '/record/lastrec.wav';

        fs.writeFileSync(wavPath, Buffer.from(wavfile));

        const stt_result = await runSTT(id, localimgpath);
        console.log("TTN결과값" + stt_result);

        // 클라이언트에게 stt_result를 JSON 형태로 응답
        res.status(200).json({ stt_result });
    } catch (error) {
        console.error('에러 발생: ', error);
        res.status(500).json({ error: 'Internal Server Error' });
    }
})

//클라리언트가 회원가입을 했을 때에 서버에 있는 모델이 학습할 수 있도록 10개의 음성파일을 전달받고, 서버에 저장하는 코드
router.post('/studyvoice', upload.any([{name: 'id', maxCount: 1}, {name: 'record', maxCount: 5}]), async(req,res)=>{
    let id = req.body.id;

    // record1부터 record10까지의 파일을 찾음
    let wavFiles = [];
    for (let i = 0; i < 5; i++) {
        if (req.files[i] && req.files[i].fieldname === 'record' + i) {
            wavFiles.push(req.files[i]);
        }
    }
    //예외처리
    if (wavFiles.length != 5) {
        res.status(400).json({result:'10개의 wav 파일이 필요합니다.'});
        return;
    }

    // 디렉토리 경로
    let dirPath = path.join(localimgpath, id, '/study');

    // 디렉토리가 없다면 생성
    if (!fs.existsSync(dirPath)) {
        fs.mkdirSync(dirPath, { recursive: true });
    }

    for(let i = 0; i < wavFiles.length; i++) {
        let wavfile = wavFiles[i].buffer;

        // 파일 경로
        let wavPath = path.join(dirPath,'record' + (i+1) + '.wav');

        // 파일 저장
        fs.writeFileSync(wavPath, Buffer.from(wavfile));
    }
    console.log("모델 학습 중")
    try{
        await Study_Model(id)
    }catch(error){
        console.log(error)
    }
    
    run_Model(id)
    res.status(200).json({result:'파일이 성공적으로 저장되었습니다.'});
});

//STT API(python)를 실행시켜주는 코드
//resolve와 reject는 return같은 개념, 하지만 reject는 오류를 리턴함 -> catch로 묶지 않으면 프로그램이 터짐
async function runSTT(id, localimgpath) {
    return new Promise((resolve, reject) => {
        const STTpythonProcess = spawn('python', ['./Stt_API/stt.py', id, localimgpath]);

        let stt_result = '';
        //결과값을 resultData안에 담는 과정
        STTpythonProcess.stdout.on('data', (data) => {
            stt_result += data.toString();
        });
        //프로세서가 끝나면 담아놓은 데이터를 resolve를 통해 return
        STTpythonProcess.on('close', (code) => {
            if (code === 0) {
                resolve(stt_result);
            } else {
                reject(`STT 프로세스 실행 중 오류 발생. 종료 코드: ${code}`);
            }
        });

        STTpythonProcess.on('error', (err) => {
            reject(`STT 프로세스 실행 중 오류 발생: ${err}`);
        });
    });
}

//파일을 생성시켜주는 코드
function createFile(directoryPath){
    fs.access(directoryPath, fs.constants.F_OK, (err) => {
        // 폴더가 존재하지 않을 경우
        if (err) {
            fs.mkdir(directoryPath, { recursive: true }, (err) => {
                if (err) throw err;
            });
        }
        
    });
}



//모델 실행시키는 함수
async function run_Model(id) {
    return new Promise((resolve, reject) => {
        const STTpythonProcess = spawn('python', ['./wake_up_model/run_model.py', id]);
        let wake_result = '';

        STTpythonProcess.stdout.on('data', (data) => {
            wake_result += data.toString();
            console.log(data.toString())
        });

        STTpythonProcess.on('close', (code) => {
            if (code === 0) {
                console.log(wake_result)
                resolve(wake_result);
            } else {
                reject(`모델 실행중 오류발생: ${code}`);
            }
        });

        STTpythonProcess.on('error', (err) => {
            reject(`모델 실행중 오류발생: ${err}`);
        });
    });
}

//모델 만드는 함수
async function Study_Model(id) {
    return new Promise((resolve, reject) => {
        const STTpythonProcess = spawn('python', ['./wake_up_model/study_model.py', id]);
        let wake_result = '';

        STTpythonProcess.stdout.on('data', (data) => {
            wake_result += data.toString();
        });

        STTpythonProcess.on('close', (code) => {
            if (code === 0) {
                console.log(wake_result)
                resolve(wake_result);
            } else {
                reject(`모델 실행중 오류발생: ${code}`);
            }
        });

        STTpythonProcess.on('error', (err) => {
            reject(`모델 실행중 오류발생: ${err}`);
        });
    });
}


module.exports =router;