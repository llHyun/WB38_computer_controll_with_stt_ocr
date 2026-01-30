const express = require('express');
const multer = require('multer');
const upload = multer();    // for parsing multipart/form-data
const router = express.Router();
const crypto = require('crypto');
const DBconnection = require('../ConnectionPool/connection.js')
const fs = require('fs/promises');
const path = require('path');

//회원탈퇴시 폴더를 삭제시켜 주는 함수
async function deleteFolderRecursive(folderPath) {
    try {
        await fs.rmdir(folderPath, { recursive: true });
        console.log(`폴더 '${folderPath}'가 삭제되었습니다.`);
    } catch (error) {
        console.error(`폴더 '${folderPath}' 삭제 중 에러 발생: ${error.message}`);
    }
}
//삭제시킬 함수의 경로
const folderPathToDelete = 'C:\\Temp\\Server\\';

// 회원 탈퇴
router.post('/withdraw',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let acc = req.body.text;
    let [id, password] = acc.split(',');

    // 비밀번호를 SHA-512로 해시하고 Base64 형식으로 반환
    let hashAlgorithm = crypto.createHash('sha512');
    let hashing = hashAlgorithm.update(password)
    let hashedString = hashing.digest('base64');
    password = hashedString;

    // 아이디에 해당하는 정보 조회하는 쿼리
    let sql = 'select password from server.account where id = ?';
    let value = [id];

    // 쿼리 실행
    DBconnection.connection.query(sql,value,function(err,result){
        // 입력받은 모든 정보들이 같을 경우에만 실행
        if(result.length > 0 && result[0].password === password){
            let del_query = 'delete from server.account where id = ?';
            let value1 = [id];
            let delete_query = 'delete from sendkey.macro where id =?';
            let value2 = [id];
            //먼저 해당 아이디로 등록된 명령어 삭제.
            DBconnection.connection.query(delete_query, value2, function(err,result){
                // 쿼리 실행중 오류
                if(err) {
                    console.error('쿼리에러:', err);
                    res.json({result:'false'});
                }
                else {
                    // 이후에 회원 정보 삭제(아이디, 비번, ...)
                    DBconnection.connection.query(del_query, value1, function(err,result){
                        // 쿼리 실행중 오류
                        if(err) {
                            console.error('쿼리에러:', err);
                            res.json({result:'false'});
                        }
                        else {
                            if (result.affectedRows > 0) {
                                deleteFolderRecursive(folderPathToDelete + id);
                                console.log(id + ' 삭제 성공');
                                res.json({ result: 'true' });
                            } else {
                                console.log(id + ' 삭제 실패');
                                res.json({ result: 'false' });
                            }
                        }
                    })
                }
            })
        }
        // 회원 정보 불일치 오류
        else{
            console.log("회원정보 불일치");
            res.json({result:'false'});
        }
    })
})

// 회원 정보 수정
router.post('/user_update',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let acc = req.body.text;
    let [id, old_pw, new_pw] = acc.split(',');

    // 비밀번호를 SHA-512로 해시하고 Base64 형식으로 반환
    let hashAlgorithm = crypto.createHash('sha512');
    let hashing = hashAlgorithm.update(old_pw)
    let hashedString = hashing.digest('base64');
    old_pw = hashedString;

    let hashAlgorithm2 = crypto.createHash('sha512');
    let hashing2 = hashAlgorithm2.update(new_pw)
    let hashedString2 = hashing2.digest('base64');
    new_pw = hashedString2;

    // 아이디에 해당하는 정보 조회하는 쿼리
    let sql = 'select password from server.account where id = ?';
    let value = [id];

    // 쿼리 실행
    DBconnection.connection.query(sql,value,function(err,result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('쿼리에러:', err);
            res.json({result:'false'});
        }
        // 아이디 존재시 입력받은 정보들을 업데이트 시켜줌
        if(result.length > 0){
            // 비밀번호 비교
            if (result[0].password === old_pw) {
                // 비밀번호가 일치할 때 업데이트 실행
                let update_query = 'update server.account set password = ? where id = ?';
                let update_value = [new_pw, id];

                DBconnection.connection.query(update_query, update_value, function (err, update_result) {
                    // 쿼리 실행중 오류
                    if (err) {
                        console.error('쿼리에러:', err);
                        res.json({ result: 'false' });
                    } else {
                        if (update_result.affectedRows > 0) {
                            console.log(id + ' 수정 성공');
                            res.json({ result: 'true' });
                        } else {
                            console.log(id + ' 수정 실패');
                            res.json({ result: 'false' });
                        }
                    }
                });
            } else {
                // 비밀번호 불일치 오류
                console.log('비밀번호 불일치');
                res.json({ result: 'false' });
            }
        }
        // 회원 정보 불일치 오류
        else{
            console.log("없는 아이디");
            res.json({result:'false'});
        }
    })
})

module.exports =router;