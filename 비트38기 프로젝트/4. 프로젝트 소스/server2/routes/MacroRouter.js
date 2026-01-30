const express = require('express');
const multer = require('multer');
const upload = multer();    // for parsing multipart/form-data
const router = express.Router();
const DBconnection = require('../ConnectionPool/connection.js')

// insert macro
router.post('/insertmacro',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let temp = req.body.text;
    let [id, cmd, key] = temp.split('#');

    let insert_query = 'INSERT INTO sendkey.macro VALUES (?, ?, ?)';
    let value = [id, cmd, key];

    DBconnection.connection.query(insert_query, value, function(err, result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('쿼리에러:', err);
            res.json({result: 'false' });
        }
        else {
            if (result.affectedRows > 0) {
                console.log(id + ' 저장 성공');
                res.json({ result: 'true' });
            } else {
                console.log(id + ' 저장 실패');
                res.json({ result: 'false' });
            }
        }
    })
})

// delete macro
router.post('/deletemacro',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let temp = req.body.text;
    let [id, key] = temp.split('#');

    let delete_query = "DELETE FROM sendkey.macro WHERE ID = ? AND macro.KEY = ?";
    let value = [id, key];

    // delete 쿼리문 실행
    DBconnection.connection.query(delete_query, value, function(err, result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('쿼리에러:', err);
            res.json({result:'false'});
        }
        else {
            if (result.affectedRows > 0) {
                console.log(id + ' 삭제 성공');
                res.json({ result: 'true' });
            } else {
                console.log(id + ' 삭제 실패');
                res.json({ result: 'false' });
            }
        }
    })
})

// update macro
router.post('/updatemacro',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let temp = req.body.text;
    let [id, ocmd, cmd, key] = temp.split('#');

    let update_query = 'UPDATE sendkey.macro SET CMD = ?, macro.KEY = ? WHERE ID = ? AND CMD = ?';
    let value = [cmd, key, id, ocmd];

    DBconnection.connection.query(update_query, value, function(err, result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('쿼리에러:', err);
            res.json({result:'false'});
        }
        else {
            if (result.affectedRows > 0) {
                console.log(id + ' 수정 성공');
                res.json({ result: 'true' });
            } else {
                console.log(id + ' 수정 실패');
                res.json({ result: 'false' });
            }
        }
    })
})

// select all to id
router.post('/selectall',upload.none(),(req,res)=>{
    let id = req.body.text;
    let selectall_query = "SELECT * FROM  sendkey.macro WHERE ID = ?";

    // selectall 쿼리문 실행
    DBconnection.connection.query(selectall_query, id, function(err, rows, result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('쿼리에러:', err);
            res.json({result:'null'});
        }
        else {
            if (rows.length > 0) { // 반환된 행의 수 확인
                console.log(id + ' selectall 성공');

                let temp = '';
                for(let i = 0; i < rows.length; i++) {
                    temp += rows[i].ID + "#";
                    temp += rows[i].CMD + "#";
                    temp += rows[i].KEY + "@";
                }
                res.json({ result : temp });
            } 
            else {
                console.log(id + ' selectall 실패');
                res.json({ result: 'null' });
            }
        }
    })
})

// select code
router.post('/selectcode',upload.none(),(req,res)=>{
    // 받은 데이터 스플릿
    let temp = req.body.text;
    let [id, cmd] = temp.split('#');

    let selectcode_query = "SELECT s.CODE FROM sendkey.sendkeys s JOIN sendkey.macro m ON m.KEY = s.KEY WHERE m.ID = ? AND m.CMD = ?";
    let value = [id, cmd];
    console.log(cmd)
    
    // selectcode 쿼리문 실행
    DBconnection.connection.query(selectcode_query, value, function(err, rows, result){
        // 쿼리 실행중 오류
        if(err) {
            console.error('쿼리에러:', err);
            res.json({result:'null'});
        }
        else {
            if (rows.length > 0) { // 반환된 행의 수 확인
                console.log(id + ' selectcode 성공');

                let temp = '';

                if (rows.length > 0) {
                    temp = rows[0].CODE;
                }
                    
                res.json({ result : temp });
            } 
            else {
                console.log(id + ' selectcode 실패');
                res.json({ result: 'null' });
            }
        }
    })
})

module.exports =router;