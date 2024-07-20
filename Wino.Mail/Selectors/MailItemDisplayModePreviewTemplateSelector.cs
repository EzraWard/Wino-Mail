﻿
using Wino.Core.Domain.Enums;

#if NET8_0
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif

namespace Wino.Selectors
{
    /// <summary>
    /// Template selector for previewing mail item display modes in Settings->Personalization page.
    /// </summary>
    public class MailItemDisplayModePreviewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CompactTemplate { get; set; }
        public DataTemplate MediumTemplate { get; set; }
        public DataTemplate SpaciousTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is MailListDisplayMode mode)
            {
                switch (mode)
                {
                    case MailListDisplayMode.Spacious:
                        return SpaciousTemplate;
                    case MailListDisplayMode.Medium:
                        return MediumTemplate;
                    case MailListDisplayMode.Compact:
                        return CompactTemplate;
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
