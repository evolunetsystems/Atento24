using System;

namespace atento24.Pages
{

    public class MasterDetailPage1MenuItem
    {
        public MasterDetailPage1MenuItem()
        {
            TargetType = typeof(MasterDetailPage1Detail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}