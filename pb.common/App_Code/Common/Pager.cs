namespace Common
{
    using System;
    using System.Text;



    public class Pager
    {
        private int absolutePage = 1;
        private int pageCount = 1;
        private string prefix = "?p=";
        private int size = 5;
        private string suffix = string.Empty;

        private string firstText = "首页";
        private string lastText = "末页";
        private string prevText = "上一页";
        private string nextText = "下一页";



        bool setFirstPageLink = false;
        private string firstPageLink = null;
        public string FirstPageLink
        {
            get
            {
                return firstPageLink;
            }
            set
            {
                firstPageLink = value;
                if (!string.IsNullOrEmpty(firstPageLink))
                {
                    setFirstPageLink = true;
                }
                else
                {
                    setFirstPageLink = false;
                }

            }
        }



        public string GetCode()
        {
            return this.GetCode("normal", "selected");
        }

        public string GetCode(string cssNormal, string cssSelect)
        {
            if (cssNormal.Trim().Length > 0)
            {
                cssNormal = " class=\"" + cssNormal + "\"";
            }
            if (cssSelect.Trim().Length > 0)
            {
                cssSelect = " class=\"" + cssSelect + "\"";
            }
            StringBuilder builder = new StringBuilder();
            int num = 1;
            int pageCount = 1;
            if (this.pageCount <= this.size)
            {
                pageCount = this.pageCount;
            }
            else
            {
                int num3 = ((this.size % 2) == 0) ? (this.size / 2) : (((this.size - 1) / 2) + 1);
                num = (this.absolutePage - num3) + 1;
                if (num < 1)
                {
                    num = 1;
                }
                pageCount = (num + this.size) - 1;
                if ((this.pageCount + 1) <= pageCount)
                {
                    pageCount = this.pageCount;
                }
            }


            for (int i = num; i <= pageCount; i++)
            {
                if (i == this.absolutePage)
                {
                    builder.Append("<strong");
                    builder.Append(cssSelect);
                    builder.Append(">");
                    builder.Append(i);
                    builder.Append("</strong>");
                }
                else
                {
                    builder.Append("<a");
                    builder.Append(cssNormal);
                    builder.Append(" href=\"");

                    if (this.setFirstPageLink && i == 1)
                    {
                        builder.Append(this.firstPageLink);
                    }
                    else
                    {
                        builder.Append(this.prefix);
                        builder.Append(i);
                        builder.Append(this.suffix);
                    }




                    builder.Append("\">");
                    builder.Append(i);
                    builder.Append("</a>");
                }
            }

            return builder.ToString();
        }

        public string GetFirst()
        {
            return this.GetFirst("first");
        }

        public string GetFirst(string css)
        {
            if (css.Trim().Length > 0)
            {
                css = " class=\"" + css + "\"";
            }



            if ((this.pageCount > this.size) && (this.absolutePage != 1))
            {
                if (this.setFirstPageLink)
                {
                    return (" <a" + css + " href=\"" + this.firstPageLink + "\">" + firstText + "</a> ");
                }
                else
                {
                    return (" <a" + css + " href=\"" + this.prefix + "1" + this.suffix + "\">" + firstText + "</a> ");
                }


            }

            return string.Empty;
        }

        public string GetLast()
        {
            return this.GetLast("last");
        }

        public string GetLast(string css)
        {
            if (css.Trim().Length > 0)
            {
                css = " class=\"" + css + "\"";
            }
            if ((this.pageCount > this.size) && (this.absolutePage < this.pageCount))
            {
                return string.Concat(new object[] { " <a", css, " href=\"", this.prefix, this.pageCount, this.suffix, "\">" + lastText + "</a> " });
            }
            return string.Empty;
        }

        public string GetNext()
        {
            return this.GetNext("next");
        }

        public string GetNext(string css)
        {
            return GetNext(css, this.nextText);
        }

        public string GetNext(string css, string text)
        {
            this.nextText = text;
            if (css.Trim().Length > 0)
            {
                css = " class=\"" + css + "\"";
            }
            if (this.absolutePage < this.pageCount)
            {
                return string.Concat(new object[] { " <a", css, " href=\"", this.prefix, this.absolutePage + 1, this.suffix, "\">" + text + "</a> " });
            }
            return string.Empty;
        }

        public string GetPrev()
        {
            return this.GetPrev("prev");
        }

        public string GetPrev(string css)
        {
            return GetPrev(css, this.prevText);
        }

        public string GetPrev(string css, string text)
        {
            this.prevText = text;
            if (css.Trim().Length > 0)
            {
                css = " class=\"" + css + "\"";
            }
            if (this.absolutePage > 1)
            {
                if (this.setFirstPageLink && this.absolutePage == 1 + 1)
                {
                    return string.Concat(new object[] { " <a", css, " href=\"", this.firstPageLink, "\">" + text + "</a> " });
                }
                else
                {
                    return string.Concat(new object[] { " <a", css, " href=\"", this.prefix, this.absolutePage - 1, this.suffix, "\">" + text + "</a> " });
                }


            }
            return string.Empty;
        }

        public string GetTotal()
        {
            return this.GetTotal("total");
        }

        public string GetTotal(string css)
        {
            if (css.Trim().Length > 0)
            {
                css = " class=\"" + css + "\"";
            }
            return string.Concat(new object[] { " <strong", css, ">", this.absolutePage, "/", this.pageCount, "</strong> " });
        }

        public override string ToString()
        {
            return (this.GetFirst("first") + this.GetPrev("prev") + this.GetCode("normal", "selected") + this.GetNext("next") + this.GetLast("last") + this.GetTotal("total"));
        }

        public int AbsolutePage
        {
            get
            {
                return this.absolutePage;
            }
            set
            {
                this.absolutePage = value;
            }
        }


        public int PageCount
        {
            get
            {
                return this.pageCount;
            }
            set
            {
                this.pageCount = value;
            }
        }

        public string Prefix
        {
            get
            {
                return this.prefix;
            }
            set
            {
                this.prefix = value;
            }
        }

        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public string Suffix
        {
            get
            {
                return this.suffix;
            }
            set
            {
                this.suffix = value;
            }
        }


        public string FirstText
        {
            get { return this.firstText; }
            set { this.firstText = value; }
        }

        public string LastText
        {
            get { return this.lastText; }
            set { this.lastText = value; }
        }

        public string PrevText
        {
            get { return this.prevText; }
            set { this.prevText = value; }
        }

        public string NextText
        {
            get { return this.nextText; }
            set { this.nextText = value; }
        }




    }


}