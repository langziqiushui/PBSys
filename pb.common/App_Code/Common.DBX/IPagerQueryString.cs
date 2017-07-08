namespace Common.DBX
{
    public interface IPagerQueryString
    {
        string GetQueryString();

        string GetCountQueryString();

        int AbsolutePage { get; set; }

        string Fields { get; set; }

        int PageSize { get; set; }

        //string Identity { get; set; }

        string Sort { get; set; }

        string Table { get; set; }

        string Where { get; set; }
    }
}