const mysql = require('mysql');
const path = require('path');

require('dotenv').config({ path: path.join(__dirname, '.env') });


const connection = mysql.createConnection({
    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_DATABASE
});

//connection을 외부에서도 사용할 수 있게 만들어주는 코드
module.exports = {connection}