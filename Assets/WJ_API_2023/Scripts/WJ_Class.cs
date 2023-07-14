namespace WjChallenge
{
    #region 진단평가 클래스 (Diagnostic Class)
    using System.Collections.Generic;
    using UnityEngine.Events;

    /// <summary>
    /// 게임 진단 문항요청에 보내는 값(진단평가 첫 실행 시)
    /// </summary>
    public class Request_DN_Setting
    {
        public string gameCd;       //게임코드
        public string mbrId;        //회원ID
        public string deviceNm;     //디바이스 이름
        public string gameVer;      //게임 버전
        public string osScnCd;      //OS 구분
        public string langCd;       //학습 언어코드
        public int timeZone;        //한국 +9
        public string bgnLvl;       //시작 수준(A,B,C,D)
    }

    /// <summary>
    /// 게임 진단 문제풀이시 보내는 값(진단평가 수행 중)
    /// </summary>
    public class Request_DN_Progress
    {
        public string gameCd;       //게임코드
        public string mbrId;        //회원ID
        public string prgsCd;       //진행코드(W : 진단진행, E : 진단완료, X : 기타취소)
        public long sid;            //진단ID
        public string qstCd;        //푼 문제의 문항 코드
        public string qstCransr;    //입력한 답 내용
        public string ansrCwYn;     //정오답 여부-Y/N
        public long slvTime;        //문제 풀이 시간(ms)

        public Request_DN_Progress()
        {
            
        }
    }

    /// <summary>
    /// 진단평가 시 받아오는 값(진단평가 첫실행, 수행중 동일)
    /// </summary>
    [System.Serializable]
    public class DN_Response
    {
        public bool result;
        public string msg;
        public Diagnotics_Data data;

        public DN_Response()
        {
            data = new Diagnotics_Data();
        }
    }

    /// <summary>
    /// 진단평가 응답 시 받아오는 문제 데이터
    /// </summary>
    [System.Serializable]
    public class Diagnotics_Data
    {
        public long sid;
        public string prgsCd;       //진행 코드(W : 진단진행, E : 진단완료)
        public string qstCd;        //문항코드
        public string qstCn;        //문항내용
        public string textCn;       //지문내용
        public string qstCransr;    //문항정답
        public string qstWransr;    //문항오답
        public int accuracy;        //진단 정확도 수준
        public int estQstNowNo;     //현재까지 문항 수(현재 문항 포함)
        public string estPreStgCd;  //적정 시작 지점의 스테이지 코드
    }

    #endregion

    #region 학습 클래스 (Learning Class)
    /// <summary>
    /// 문제풀이(학습) 시 문항요청에 보내는 값
    /// </summary>
    [System.Serializable]
    public class Request_Learning_Setting
    {
        public string gameCd;
        public string mbrId;
        public string gameVer;
        public string osScnCd;
        public string deviceNm;
        public string langCd;
        public int timeZone;
        public string mathpidId;
    }

    /// <summary>
    /// 문제풀이(학습) 요청 시 받아오는 값
    /// </summary>
    [System.Serializable]
    public class Response_Learning_Setting
    {
        public bool result;
        public string msg;
        public Response_Learning_SettingData data;
    }

    [System.Serializable]
    public class Response_Learning_SettingData
    {
        public long sid;
        public string bgnDt;
        public List<Learning_Question> qsts;
    }

    /// <summary>
    /// 문제풀이(학습) 완료 시 보내는 값
    /// </summary>
    public class Request_Learning_Progress
    {
        public string gameCd;
        public string mbrId;
        public string prgsCd;
        public long sid;
        public string bgnDt;

        public List<Learning_MyAnsr> data;

        public Request_Learning_Progress()
        {
            data = new List<Learning_MyAnsr>();
        }
    }
    [System.Serializable]
    public class Response_Learning_Progress
    {
        public bool result;
        public string msg;
        public Response_Learning_ProgressData data;
    }

    /// <summary>
    /// 문제풀이 결과
    /// </summary>
    [System.Serializable]
    public class Response_Learning_ProgressData
    {
        public string explSpedCd;
        public int explSped;
        public string lrnPrgsStsCd;
        public string acrcyCd;
        public int explAcrcyRt;
    }

    /// <summary>
    /// 문제풀이 요청 시 받아오는 개별 문제
    /// </summary>
    [System.Serializable]
    public class Learning_Question
    {
        public string qstCd;
        public string qstCn;
        public string textCn;
        public string qstCransr;
        public string qstWransr;
    }

    /// <summary>
    /// 문제풀이 완료 시 보내는 개별 답안
    /// </summary>
    [System.Serializable]
    public class Learning_MyAnsr
    {
        public string qstCd;
        public string qstCransr;
        public string ansrCwYn;
        public long slvTime;

        public Learning_MyAnsr(string _cd, string _cransr, string _cwyn, long _time)
        {
            qstCd = _cd;
            qstCransr = _cransr;
            ansrCwYn = _cwyn;
            slvTime = _time;
        }
    }
    #endregion
}
