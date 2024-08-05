namespace Fashion_Avenue.Models
{
    public class IndexModel
    {
        public IEnumerable<Fashion_Avenue.Models.Blog> Blog { get; set; }

        public IEnumerable<Fashion_Avenue.Models.Product> Product { get; set; }
        public IEnumerable<Fashion_Avenue.Models.ProductLike> ProductLike { get; set; }
        public int Product_Count { get; set; }
        public IEnumerable<Fashion_Avenue.Models.LikeCount> LikeCount { get; set; }
        public IEnumerable<Fashion_Avenue.Models.ProductColor> ProductColor { get; set; }

        public IEnumerable<Fashion_Avenue.Models.Category> Category { get; set; }
        public IEnumerable<Fashion_Avenue.Models.BlogComment> BlogComment { get; set; }
        public Fashion_Avenue.Models.Blog BlogDetail { get; set; }



    }
}
