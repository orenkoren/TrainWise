using System;
using System.Collections.Generic;
using System.Text;

namespace TrainWise.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Greet
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
