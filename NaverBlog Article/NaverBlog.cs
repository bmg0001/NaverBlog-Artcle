using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using WinHttp;

namespace NaverBlog_Article
{
    public class NaverBlog
    {
        public bool Post(string NaverID,string NaverPW,string title,string content,int category)
        {
            try
            {
                content = HttpUtility.UrlEncode(content,Encoding.GetEncoding(949));
                title = HttpUtility.UrlEncode(title, Encoding.GetEncoding(949));
                WinHttpRequest ht = new WinHttpRequest();
                ht.Open("POST", "https://nid.naver.com/nidlogin.login");
                ht.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                ht.SetRequestHeader("Referer", "https://nid.naver.com/nidlogin.login");
                ht.Send("enctp=2&url=http://www.naver.com&enc_url=http://www.naver.com&postDataKey=&saveID=0&nvme=0&smart_level=1&id=" + NaverID + "&pw=" + NaverPW);
                ht.WaitForResponse();
                string[] cookie = ht.GetAllResponseHeaders().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int cookieCount = 0;
                foreach (String header in cookie)
                {
                    if (header.StartsWith("Set-Cookie: "))
                    {
                        cookieCount++;
                    }
                }
                String[] cookies = new String[cookieCount];
                cookieCount = 0;
                foreach (String header in cookie)
                {
                    if (header.StartsWith("Set-Cookie: "))
                    {
                        String cookie1 = header.Replace("Set-Cookie: ", "");
                        cookies[cookieCount] = cookie1;
                    }
                }
                string Fcookie = "";

                foreach (string cds in cookies)
                {
                    Fcookie += cds;
                }
                //로그인 종료

                ht.Open("GET", "http://blog.naver.com/PostWriteForm.nhn?blogId="+NaverID+"&Redirect=Write&useSmartEditorVersion=2&redirect=Write&widgetTypeCall=true");
                ht.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                ht.SetRequestHeader("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");
                ht.SetRequestHeader("Cookie", Fcookie);
                ht.SetRequestHeader("Host", "blog.naver.com");
                ht.SetRequestHeader("Referer", "http://blog.naver.com/" + NaverID + "?Redirect=Write");
                ht.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                ht.Send();
                string blogNo = Regex.Split(Regex.Split(ht.ResponseText, "var blogNo = '")[1], "';")[0];
                string nowYear = Regex.Split(Regex.Split(ht.ResponseText, "var nowYear = \"")[1], "\";")[0];
                string nowMount = Regex.Split(Regex.Split(ht.ResponseText, "var nowMonth = \"")[1], "\";")[0];
                string nowDate = Regex.Split(Regex.Split(ht.ResponseText, "var nowDate = \"")[1], "\";")[0];
                string nowHour = Regex.Split(Regex.Split(ht.ResponseText, "var nowHour = \"")[1], "\";")[0];
                string nowMinute = Regex.Split(Regex.Split(ht.ResponseText, "var nowMinute = \"")[1], "\";")[0];
                string editorSource = Regex.Split(Regex.Split(ht.ResponseText, "name=\"editorSource\" value=\"")[1], "\">")[0];

                ht.Open("POST", "http://blog.naver.com/TempPostWriteAsync.nhn");
                ht.SetRequestHeader("Accept", "*/*");
                ht.SetRequestHeader("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");
                ht.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                ht.SetRequestHeader("Cookie", Fcookie);
                ht.SetRequestHeader("Host", "blog.naver.com");
                ht.SetRequestHeader("Referer", "http://blog.naver.com/" + NaverID + "/postwrite");
                ht.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                string postdata = "captchaKey=&captchaValue=&appId=&tempLogNo=&blogId=" + NaverID + "&post.logNo=&post.sourceCode=0&post.contents.contentsValue=" + content + "&post.prePostRegistDirectly=false&post.lastRelayTime=&smartEditorVersion=2&post.book.ratingScore=0&post.music.ratingScore=0&post.movie.ratingScore=0&post.scrapedYn=false&post.clientType=&post.contents.summaryYn=false&post.contents.summaryToggleText=&post.contents.summaryTogglePosition=&post.templatePhoto.width=0&post.templatePhoto.height=0&post.addedInfoSet.addedInfoStruct=&post.mapAttachmentSet.mapAttachStruct=&post.calendarAttachmentSet.calendarAttachmentStruct=&post.musicPlayerAttachmentSet.musicPlayerAttachmentStruct=&post.eventWriteInfo.eventCode=&post.eventWriteInfo.writeCode=&post.eventWriteInfo.eventType=&post.eventWriteInfo.eventLink=&post.postOptions.openType=2&post.postOptions.commentYn=true&post.postOptions.isRelayOpen=&post.postOptions.sympathyYn=true&post.postOptions.outSideAllowYn=true&post.me2dayPostingYn=&post.facebookPostingYn=false&post.twitterPostingYn=false&post.postOptions.searchYn=true&post.postOptions.rssOpenYn=true&post.postOptions.scrapType=2&post.postOptions.ccl.commercialUsesYn=false&post.postOptions.ccl.contentsModification=&post.postOptions.noticePostYn=false&directorySeq=0&directoryDetail=&post.bookTheme.infoPk=&post.movieTheme.infoPk=&post.musicTheme.infoPk=&post.kitchenTheme.recipeName=&post.postOptions.directoryOptions.directoryChangeYn=false&post.postOptions.directoryOptions.tagAutoChangedYn=false&post.postOptions.isAutoTaggingEnabled=true&post.postOptions.greenReviewBannerYn=false&previewGreenReviewBannerAsInteger=0&post.leverageOptions.themeSourceCode=&post.music.subType=&post.postOptions.isContinueSaved=false&post.mrBlogTalk.talkCode=&happyBeanGiveDayReqparam=&post.postOptions.isExifEnabled=false&editorSource=" + editorSource + "&post.category.categoryNo=" + category.ToString() + "&post.title=" + title + "&ir1=%0A%0A&query=%EC%A7%80%EC%97%AD%EB%AA%85%EC%9D%84%20%EC%9E%85%EB%A0%A5%ED%95%B4%20%EC%A3%BC%EC%84%B8%EC%9A%94&char_preview=%C2%AE%C2%BA%E2%8A%86%E2%97%8F%E2%97%8B&se2_tbp=on&se2_tbp3=on&=on&post.directorySeq=0&post.tag.names=%ED%83%9C%EA%B7%B8%EC%99%80%20%ED%83%9C%EA%B7%B8%EB%8A%94%20%EC%89%BC%ED%91%9C%EB%A1%9C%20%EA%B5%AC%EB%B6%84%ED%95%98%EB%A9%B0%2C%2010%EA%B0%9C%EA%B9%8C%EC%A7%80%20%EC%9E%85%EB%A0%A5%ED%95%98%EC%8B%A4%20%EC%88%98%20%EC%9E%88%EC%8A%B5%EB%8B%88%EB%8B%A4.&openType=2&post.postWriteTimeType=now&prePostDay=" + DateTime.Now.ToString("yyyy년 MM월 dd일") + "&prePostDateType.hour=" + nowHour + "&prePostDateType.minute=" + nowMinute + "&prePostDateType.year=&prePostDateType.month=&prePostDateType.date=&commercialUses=false&contentsModification=0&writingMaterialInfos=%5B%5D";
                ht.Send(postdata);
                string postlogNo = ht.ResponseText;

                ht.Open("POST", "http://blog.naver.com/PostWrite.nhn");
                ht.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                ht.SetRequestHeader("Accept-Encoding", "deflate");
                ht.SetRequestHeader("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");
                ht.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                ht.SetRequestHeader("Cookie", Fcookie);
                ht.SetRequestHeader("Host", "blog.naver.com");
                ht.SetRequestHeader("Referer", "http://blog.naver.com/" + NaverID + "/postwrite");
                ht.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                //string postdata2 = "captchaKey=&captchaValue=&appId=&tempLogNo=&blogId=" + NaverID + "&post.logNo=" + postlogNo + "&post.sourceCode=0&post.contents.contentsValue=<p>" + content + "</p>&post.prePostRegistDirectly=false&post.lastRelayTime=&smartEditorVersion=2&post.book.ratingScore=0&post.music.ratingScore=0&post.movie.ratingScore=0&post.scrapedYn=false&post.clientType=&post.contents.summaryYn=false&post.contents.summaryToggleText=&post.contents.summaryTogglePosition=&post.templatePhoto.width=0&post.templatePhoto.height=0&post.addedInfoSet.addedInfoStruct=&post.mapAttachmentSet.mapAttachStruct=&post.calendarAttachmentSet.calendarAttachmentStruct=&post.musicPlayerAttachmentSet.musicPlayerAttachmentStruct=&post.eventWriteInfo.eventCode=&post.eventWriteInfo.writeCode=&post.eventWriteInfo.eventType=&post.eventWriteInfo.eventLink=&post.postOptions.openType=2&post.postOptions.commentYn=true&post.postOptions.isRelayOpen=&post.postOptions.sympathyYn=true&post.postOptions.outSideAllowYn=true&post.me2dayPostingYn=&post.facebookPostingYn=false&post.twitterPostingYn=false&post.postOptions.searchYn=true&post.postOptions.rssOpenYn=true&post.postOptions.scrapType=2&post.postOptions.ccl.commercialUsesYn=false&post.postOptions.ccl.contentsModification=&post.postOptions.noticePostYn=false&directorySeq=0&directoryDetail=&post.bookTheme.infoPk=&post.movieTheme.infoPk=&post.musicTheme.infoPk=&post.kitchenTheme.recipeName=&post.postOptions.directoryOptions.directoryChangeYn=false&post.postOptions.directoryOptions.tagAutoChangedYn=false&post.postOptions.isAutoTaggingEnabled=true&post.postOptions.greenReviewBannerYn=false&previewGreenReviewBannerAsInteger=0&post.leverageOptions.themeSourceCode=&post.music.subType=&post.postOptions.isContinueSaved=false&post.mrBlogTalk.talkCode=&happyBeanGiveDayReqparam=&post.postOptions.isExifEnabled=false&editorSource=" + editorSource + "&post.category.categoryNo=" + category.ToString() + "&post.title=" + title + "&ir1=<p>" + content + "</p>&query=%C1%F6%BF%AA%B8%ED%C0%BB+%C0%D4%B7%C2%C7%D8+%C1%D6%BC%BC%BF%E4&char_preview=%A2%E7%A8%AC%A1%F6%A1%DC%A1%DB&se2_tbp=on&se2_tbp3=on&post.directorySeq=0&post.tag.names=&openType=2&post.postWriteTimeType=now&prePostDay=2016%B3%E2+5%BF%F9+18%C0%CF&prePostDateType.hour=" + nowHour + "&prePostDateType.minute=" + nowMinute + "&prePostDateType.year=" + nowYear + "&prePostDateType.month=" + nowMount + "&prePostDateType.date=" + nowDate + "&commercialUses=false&contentsModification=0&writingMaterialInfos=%5B%5D";
                string postdata2 = "captchaKey=&captchaValue=&appId=&tempLogNo=&blogId=" + NaverID + "&post.logNo=" + postlogNo + "&post.sourceCode=0&post.contents.contentsValue=" + content + "&post.prePostRegistDirectly=false&post.lastRelayTime=&smartEditorVersion=2&post.book.ratingScore=0&post.music.ratingScore=0&post.movie.ratingScore=0&post.scrapedYn=false&post.clientType=&post.contents.summaryYn=false&post.contents.summaryToggleText=&post.contents.summaryTogglePosition=&post.templatePhoto.width=0&post.templatePhoto.height=0&post.addedInfoSet.addedInfoStruct=&post.mapAttachmentSet.mapAttachStruct=&post.calendarAttachmentSet.calendarAttachmentStruct=&post.musicPlayerAttachmentSet.musicPlayerAttachmentStruct=&post.eventWriteInfo.eventCode=&post.eventWriteInfo.writeCode=&post.eventWriteInfo.eventType=&post.eventWriteInfo.eventLink=&post.postOptions.openType=2&post.postOptions.commentYn=true&post.postOptions.isRelayOpen=&post.postOptions.sympathyYn=false&post.postOptions.outSideAllowYn=true&post.me2dayPostingYn=&post.facebookPostingYn=false&post.twitterPostingYn=false&post.postOptions.searchYn=true&post.postOptions.rssOpenYn=true&post.postOptions.scrapType=2&post.postOptions.ccl.commercialUsesYn=false&post.postOptions.ccl.contentsModification=&post.postOptions.noticePostYn=false&directorySeq=0&directoryDetail=&post.bookTheme.infoPk=&post.movieTheme.infoPk=&post.musicTheme.infoPk=&post.kitchenTheme.recipeName=&post.postOptions.directoryOptions.directoryChangeYn=false&post.postOptions.directoryOptions.tagAutoChangedYn=false&post.postOptions.isAutoTaggingEnabled=true&post.postOptions.greenReviewBannerYn=false&previewGreenReviewBannerAsInteger=0&post.leverageOptions.themeSourceCode=&post.music.subType=&post.postOptions.isContinueSaved=false&post.mrBlogTalk.talkCode=&happyBeanGiveDayReqparam=&post.postOptions.isExifEnabled=false&editorSource=" + editorSource + "&post.category.categoryNo=" + category.ToString() + "&post.title=" + title + "&ir1=" + content + "&query=%C1%F6%BF%AA%B8%ED%C0%BB+%C0%D4%B7%C2%C7%D8+%C1%D6%BC%BC%BF%E4&char_preview=%A2%E7%A8%AC%A1%F6%A1%DC%A1%DB&se2_tbp=on&se2_tbp3=on&post.directorySeq=0&post.tag.names=&openType=2&post.postWriteTimeType=now&prePostDay=2016%B3%E2+5%BF%F9+22%C0%CF&prePostDateType.hour=" + nowHour + "&prePostDateType.minute=" + nowMinute + "&prePostDateType.year=" + nowYear + "&prePostDateType.month=" + nowMount + "&prePostDateType.date=" + nowDate + "&commercialUses=false&contentsModification=0&writingMaterialInfos=%5B%5D";
                ht.Send(postdata2);
                if (ht.ResponseText.IndexOf("refresh") != -1)
                {
                    return true;
                }
                else
                {
                    if(ht.ResponseText.IndexOf("비정상적") != -1)
                    {
                        Console.WriteLine("[E01] 네이버 블로그서비스 에서 등록거부를 Return 받았습니다.");
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
