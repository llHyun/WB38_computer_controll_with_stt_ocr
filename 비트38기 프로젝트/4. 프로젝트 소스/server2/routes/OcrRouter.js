const express = require('express');
const fs = require('fs');
const spawn = require('child_process').spawn;
const multer = require('multer');
const upload = multer();    // for parsing multipart/form-data
const router = express.Router();
const localimgpath = "C:\\Temp\\Server"

//upload post 
//OCR 요청을 받는 post
router.post('/upload', upload.fields([{name: 'id', maxCount: 1}, {name: 'result', maxCount: 1}]),async(req,res)=>{
    let id = req.body.id;
    let result = req.body.result;
    //클라이언트한테 받은 이미지 배열과 텍스트
    console.log('id : ', id);
    console.log('텍스트 : ', result);

    imagePath = localimgpath + "/" + id + "/screenshot.jpg"

    try{
        const Ocr_result = await runOCR(result, imagePath)
        //좌표 두개 이상 저장됨
        if(Ocr_result.includes('|')){
            res.json({
                texts: Ocr_result
            });
        }
            //좌표 하나만 저장됨
        else if(Ocr_result.includes(',')) {
            res.json({
                //좌표값 전송
                text: Ocr_result
            })
        }//'|', ',' 둘다 없다면 에러
        else{
            res.json({
                emptyocr : "결과 없음"
            })
        }
    }catch(error){
        res.json({
            emptyocr : "결과 없음"
        })
    }
    
})

//OCR api(python)을 실행시키는 함수
//resolve와 reject는 return같은 개념, 하지만 reject는 오류를 리턴함 -> catch로 묶지 않으면 프로그램이 터짐
async function runOCR(text, localimgpath) {
    return new Promise((resolve, reject) => {
        const pythonProcess = spawn('python', ['./OCR_Google/app.py', text, localimgpath]);

        let resultData = '';

        //stdout
        //OCR결과값을 resultData안에 담는 과정
        pythonProcess.stdout.on('data',(data)=>{

            console.log(data.toString());
            resultData += data.toString();
        })
        //프로세서가 끝나면 담아놓은 데이터를 resolve를 통해 return
        pythonProcess.on('close', (code) => {
            if (code === 0) {
                resolve(resultData);
            } else {
                reject(`OCR 프로세스 실행 중 오류 발생. 종료 코드: ${code}`);
            }
        });

        pythonProcess.on('error', (err) => {
            reject(`OCR 프로세스 실행 중 오류 발생: ${err}`);
        });
    });
}

module.exports =router;