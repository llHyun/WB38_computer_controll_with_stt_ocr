//Control.cs
using Client3.Http;
using Client3.Member;
using Client3.WAV_Record;
using Client3.WinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3
{
    internal class Control
    {
        private Record rec = new Record();
        private Screenshot screenshot = new Screenshot();
        private WAV_Http ocr_http = new WAV_Http();
        private Member_Http memhttp = new Member_Http();
        private Macro_Http MacroHttp = new Macro_Http();
        private MouseController mouse = new MouseController();
        private ChangeText ct = new ChangeText();
        private LabelForm lf = new LabelForm();
        private progress prog = new progress();

        public List<Coordinate> coordinates = new List<Coordinate>();
        public List<Macro> mList = new List<Macro>();
        public string Id { get; set; } = string.Empty;
        public Form LabelForm { get; set; }
        public Process recProcess {  get; set; }
        public Process studyProcess { get; set; }
        public bool IsSpeaker { get; set; }
        public bool IsLabel { get; set; } = false;

        #region 싱글톤
        public static Control Singleton;
        static Control()
        {
            Singleton = new Control();
        }
        // 생성자
        private Control()
        {
            DeleteRecord();
            MakeFile();
        }
        #endregion

        #region 파일 생성
        public void MakeFile()
        {
            string wavFilePath = "C:\\Temp\\client\\record\\lastrec.wav";
            string wavDirectory = Path.GetDirectoryName(wavFilePath);
            string studyDirectory = "C:\\Temp\\client\\study_voice";

            // WAV 파일 디렉토리 생성
            if (!Directory.Exists(wavDirectory))
            {
                Directory.CreateDirectory(wavDirectory);
            }
            if (!Directory.Exists(studyDirectory))
            {
                Directory.CreateDirectory(studyDirectory);
            }
            // WAV 파일 생성
            try
            {
                using (FileStream fs = File.Create(wavFilePath))
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the WAV file: {ex.Message}");
            }
        }
        #endregion

        #region 소멸자
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~Control()
        {
            Dispose();
        }
        #endregion

        #region Load, Closing
        public void Load()
        {
            Task.Run(() => rec.RecordStart());
        }

        public void Closing()
        {
            rec.RecordEnd();
        }
        
        public async void GetInfo()
        {
            Id = await getID();
            mList = await SelectAllToID(Id);
        }
        #endregion

        #region 마우스
        //좌표로 마우스 이동
        public void mouse_Move(int x, int y)
        {
            mouse.Move(x, y);
        }
        //마우스에 클릭 이벤트
        public void mouse_Click()
        {
            mouse.Click();
        }
        //마우스에 더블 클릭 이벤트
        public void mouse_DClick()
        {
            mouse.DoubleClick();
        }
        #endregion

        #region 로그인&회원가입
        //회원가입
        public async Task<bool> signup(string accinfo)
        {
            if (await memhttp.Signup(accinfo) == true)
            {
                return true;
            }
            else { return false; }
        }
        //로그인
        public async Task<(bool, string)> Login(string idset)
        {
            var result = await memhttp.Login(idset);
            if(result.Item1==true)
            {
                return (true, result.Item2);
            }
            else { return (false, null); }
        }
        //자동 로그인
        public async Task<(bool, string)> auto(string token)
        {
            var result = await memhttp.auto(token);
            if( result.Item1 == true)
            {
                return (true, result.Item2);
            }
            else { return (false, null); }
        }
        #endregion

        #region 회원 탈퇴, 수정
        // 회원 탈퇴
        public async Task<bool> Withdraw(string accinfo)
        {
            if (await memhttp.Withdraw(accinfo) == true)
            {
                return true;
            }
            else { return false; }
        }

        //정보 수정
        public async Task<bool> Update(string accinfo)
        {
            if (await memhttp.Update(accinfo) == true)
            {
                return true;
            }
            else { return false; }
        }
        #endregion

        #region 사용자 ID 획득
        public async Task<string> getID()
        {
            if (File.Exists("token.dat"))
            {
                string protectedToken = File.ReadAllText("token.dat");
                var result = await auto(protectedToken);
                return result.Item2;
            }

            return null;
        }
        #endregion

        #region Screenshot
        public void ScreenShot()
        {
            screenshot.Capture();
        }
        #endregion

        #region http
        public async Task SendWAV(string id, string bytepic, string filepath)
        {
            await ocr_http.SendWAV(id, bytepic, filepath);
        }

        public async Task SendTTN(string id, string filepath)
        {
            await ocr_http.SendTTN(id, filepath);
        }
        #endregion

        #region MacroHttp
        // insert
        public async Task<bool> InsertMacro(string id, string cmd, string key)
        {
            return await MacroHttp.InsertMacro(id, cmd, key);
        }

        // delete
        public async Task<bool> DeleteMacro(string id, string key)
        {
            return await MacroHttp.DeleteMacro(id, key);
        }

        // update
        public async Task<bool> UpdateMacro(string id, string ocmd, string cmd, string key)
        {
            return await MacroHttp.UpdateMacro(id, ocmd, cmd, key);
        }


        // selectall
        public async Task<List<Macro>> SelectAllToID(string id)
        {
            string value = await MacroHttp.SelectAllToID(id);

            if (value == null)
            {
                MessageBox.Show("value에 값이 존재하지 않습니다.");
                return null;
            }

            string[] sp = value.Split('@');

            List<string> list = new List<string>();

            for (int i = 0; i < sp.Length - 1; i++)
            {
                if (sp[i] != "null") // 빈 문자열이 아닌 경우에만 리스트에 추가
                {
                    list.Add(sp[i]);
                }
            }


            foreach (string item in list)
            {
                if (item != "null") // 빈 문자열이 아닌 경우에만 리스트에 추가
                {
                    string[] sp2 = item.Split('#');

                    Macro m = new Macro(sp2[1], sp2[2]);

                    mList.Add(m);
                }
            }

            return mList;
        }
        #endregion

        #region change text
        public int ChangeToNum(string input)
        {
            return ct.ChangeToNum(input);
        }
        public int FindNum(string input)
        {
            return ct.FindNum(input);
        }
        #endregion

        #region Label Form
        public void Label_Form(List<Coordinate> text, Control con)
        {
            lf.Label_Form(text, con);
        }
        #endregion

        #region 녹음파일 삭제
        public void DeleteRecord()
        {
            string filePath = "C:\\Temp\\client\\record\\lastrec.wav";
            try
            {
                // 파일 삭제
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"File deleted: {filePath}");
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
