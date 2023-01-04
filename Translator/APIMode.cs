namespace Translator
{
    public partial class MainWindow
    {
        enum APIMode { Translator, LangDetect }
        enum LanguageBoxType { source, target }
        enum TranslateAPIType
        {
            Papago,
            Google,
            Kakao,
            Bing,
            Watson,
            Yandex,
            Systran,
            Baidu
        }
        enum LanguageCode
        {
            ko = 1,     //한국어
            ja = 2,     //일본어
            zh_CN = 3,  //중국어(간체)
            zh_TW = 4,  //중국어(번체)
            hi = 5,     //힌디어
            en = 6,     //영어
            es = 7,     //스페인어
            fr = 8,     //프랑스어
            de = 9,     //독일어
            pt = 10,    //포르투갈어
            vi = 11,    //베트남어
            id = 12,    //인도네시아어
            fa = 13,    //페르시아어
            ar = 14,    //아랍어
            mm = 15,    //미얀마어
            th = 16,    //태국어
            ru = 17,    //러시아어
            it = 18,    //이탈리아어
            unk = 19    //알 수 없음
        }
    }
}
