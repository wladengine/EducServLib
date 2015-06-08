using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EducServLib
{
    public class MarkClass
    {   
        // возвращает оценки прописью
        public static string MarkProp(string mark)
        {
            switch (mark)
            {
                case "2":
                    return "два";
                case "3":
                    return "три";
                case "4":
                    return "четыре";
                case "5":
                    return "пять";
                case "зачет":
                    return "зачет";
                case "незачет":
                    return "незачет";
                case "1":
                    return "зачет";
                case "0":
                    return "незачет";
                default:
                    return "";
            }
        }
    }
}
