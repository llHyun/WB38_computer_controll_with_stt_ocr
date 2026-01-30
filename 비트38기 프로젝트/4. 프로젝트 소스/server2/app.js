const express = require('express');
const app = express();
const LoginRouter = require("./routes/LoginRouter.js");
const UserMenuRouter = require("./routes/UserMenuRouter.js");
const MacroRouter = require("./routes/MacroRouter.js");
const SttRouter = require("./routes/Stt_TellRouter.js");
const OcrRouter = require("./routes/OcrRouter.js")
const bodyParser = require('body-parser');


app.use(express.json());
app.use(bodyParser.json());
app.use("/LoginRouter", LoginRouter); //로그인 라우터
app.use("/UserMenuRouter", UserMenuRouter);
app.use("/MacroRouter", MacroRouter);
app.use("/SttRouter", SttRouter);
app.use("/OcrRouter", OcrRouter)


app.listen(3000, ()=>{
    console.log('3000포트에서 서버 실행중')
})

module.exports = app;
