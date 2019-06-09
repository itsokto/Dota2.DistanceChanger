using System;
using System.Windows.Markup;
using Dota2.DistanceChanger.Core.Abstractions;
using MaterialDesignThemes.Wpf;

namespace Dota2.DistanceChanger.Infrastructure
{
    public class UserDialog : MarkupExtension, IUserDialogs
    {
        private static readonly ISnackbarMessageQueue SnackbarMessageQueue = new SnackbarMessageQueue();

        public void Alert(object content)
        {
            SnackbarMessageQueue.Enqueue(content);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return SnackbarMessageQueue;
        }
    }
}