using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaverBlog_Article;

namespace NaverBlog_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            NaverBlog nv = new NaverBlog();
            Console.WriteLine(nv.Post("","", "", "", 1)); 
            //nv.Post("네이버 아이디","네이버 비밀번호","제목","내용",Int32 형식의 카테고리ID)
            Console.ReadLine();
        }
    }
}
