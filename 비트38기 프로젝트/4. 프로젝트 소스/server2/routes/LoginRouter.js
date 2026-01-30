const express = require('express');
const multer = require('multer');
const upload = multer();    // for parsing multipart/form-data
const router = express.Router();
const fs = require('fs');
const spawn = require('child_process').spawn;
const crypto = require('crypto');
const jwt = require('jsonwebtoken');
var privateKey = '자동로그인autologin123!@#';
const DBconnection = require('../ConnectionPool/connection.js')

//모든 모델 시작
startmodel()

//회원가입
router.post('/signup',upload.none(),(req,res)=>{

    // 받은 데이터 스플릿으로 잘라냄
    let accinfo = req.body.text;
    let [id, password, name, phone, email] = accinfo.split(',');

    // 아이디 중복을 확인하는 쿼리문
    let checkIdQuery = 'SELECT * FROM account WHERE id = ?';
    DBconnection.connection.query(checkIdQuery, [id], (err, result) => {
        // 아이디 조회 중 오류 발생했을때
        if (err) {
            console.error('ID 조회 에러:', err);
            res.json({ result: 'false' });
        }
        // 아이디가 이미 존재했을때
        if (result.length > 0) {
            console.error('이미 존재하는 ID입니다.');
            res.json({ result: 'duplication' });
        }

        // 비밀번호를 SHA-512로 해시하고 Base64 형식으로 변환함.
        let hashAlgorithm = crypto.createHash('sha512');
        let hashing = hashAlgorithm.update(password)
        let hashedString = hashing.digest('base64');
        password = hashedString;

        // 회원 정보를 디비에 저장하는 쿼리
        let sql = 'insert into account(id, password, name, phone, email,regdata) values(?,?,?,?,?,now())'
        let values = [id, password, name, phone, email];

        //쿼리 실행
        DBconnection.connection.query(sql, values, function(err,result) {
            // 쿼리 실행중 오류 발생했을때
            if (err) {
                console.error('계정 삽입 에러:', err);
                res.json({result: 'false'});
            }
            // 실행 성공
            createFile("C:\\Temp\\Server\\" + id);
            console.log('계정 1개를 삽입하였습니다.')
            res.json({result: 'true'});
        })
    })
})

//자동로그인
router.post('/autologin',upload.none(), (req, res) => {
    // 클라이언트로부터 받은 토큰 추출
    let token = req.body.token;

    // 토큰 제공되지 않았을때
    if (!token) {
        console.error('No token provided');
        res.json({result:'false'});
    }

    // JWT 토큰을 검증
    jwt.verify(token, privateKey, (err, decoded) => {
        // 토큰 검증에 실패
        if (err) {
            console.error(err);
            res.json({result:'false'});
        }
        // 토큰 검증 성공
        // 토큰에서 사용자 id추출 / 결과와 id 반환
        else {
            let userId = decoded.id;
            let checkIdQuery = 'SELECT * FROM account WHERE id = ?';
            DBconnection.connection.query(checkIdQuery, [userId], (err, result) => {
                // 아이디 조회 중 오류 발생했을때
                if (err) {
                    console.error('ID 조회 에러:', err);
                    res.json({result: 'false'});
                }
                // 아이디가 이미 존재했을때
                else if (result.length > 0) {
                    createFile("C:\\Temp\\Server\\" + userId);
                    console.log(userId + '로그인 성공');
                    res.json({result: 'true', id: userId});
                } else {
                    res.json({result: 'false'});
                }
            })
        }
    });
})

//로그인
router.post('/login',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let acc = req.body.text;
    let [id, password] = acc.split(',');

    // 비밀번호를 SHA-512로 해시하고 Base64 형식으로 반환
    let hashAlgorithm = crypto.createHash('sha512');
    let hashing = hashAlgorithm.update(password)
    let hashedString = hashing.digest('base64');
    password = hashedString;

    // 아이디에 해당하는 비밀번호 조회하는 쿼리
    let sql = 'SELECT password FROM account WHERE id = ?';
    let value = [id];

    // 쿼리 실행
    DBconnection.connection.query(sql,value,function(err,result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('로그인에러:', err);
            res.json({result:'false'});
        }
        // 아이디 존재시 비번확인
        if(result.length > 0){
            // 비번 일치시 JWT 토큰 생성해서 결과와 함께 반환
            if(result[0].password===password){
                console.log(id +'로그인 성공');
                createFile("C:\\Temp\\Server\\" + id);
                var token = jwt.sign({id: id}, privateKey,{expiresIn:'1d'});
                res.json({result:'true', token: token, id: id});
            }
            // 비밀번호 불일치 오류
            else{
                console.log("비밀번호 불일치");
                res.json({result:'false'});
            }
        }
        // 아이디 없을시 오류
        else {
            console.log("id 없음");
            res.json({result:'false'});
        }
    });
})

//서버에 있는 사용자의 정보를 받아와 전체 모델 실행
function startmodel(){
    let sql = 'SELECT id FROM account';
    // 쿼리 실행
    DBconnection.connection.query(sql,function(err,result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('로그인에러:', err);
        }
        // 아이디 존재시 비번확인
        if(result.length > 0){
            for(i=0;i<result.length;i++){
                run_Model(result[i].id)
                console.log(result[i].id)
            }
        }
        // 아이디 없을시 오류
        else {
            console.log("id 없음");
        }
    });
}

//폴더 만드는 함수
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