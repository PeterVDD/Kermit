﻿using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
//using Willow.Kermit.General.Interfaces;
using Willow.Kermit.Searching.ViewModels;
using Willow.Kermit.Util;

namespace Willow.Kermit.Searching.Exports
{
    //[Export(typeof(ITabViewModelFactory))]
    //[Export(typeof(ISearchResultsFactory))]
    public class SearchResultsViewModelFactory // : ISearchResultsFactory
    {
        public BitmapImage Image
        {
            get { return new DefaultImageGetter().Search; }
        }

        public string Caption
        {
            get { return "Zoeken"; }
        }

        //public ITabViewModel Create()
        //{
        //    return new SearchResultsViewModel();
        //}

        //public ISearchResults Create(string searchText)
        //{
        //    return new SearchResultsViewModel { SearchString = searchText };
        //}
    }

}