using System;
using System.Threading.Tasks;
using System.Windows.Markup;
using Dota2.DistanceChanger.Core.Abstractions;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace Dota2.DistanceChanger.Platform
{
    public class UserDialog : MarkupExtension, IUserDialogs
    {
        private static readonly ISnackbarMessageQueue SnackbarMessageQueue = new SnackbarMessageQueue();

        public void Alert(object content)
        {
            SnackbarMessageQueue.Enqueue(content);
        }

        public async Task<string> OpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog();

            var result = openFileDialog.ShowDialog();

            return result.HasValue && result.Value ? openFileDialog.FileName : string.Empty;
        }

        public async Task<string> OpenFolderDialog()
        {
            var openBrowserDialog = new VistaFolderBrowserDialog();

            var result = openBrowserDialog.ShowDialog();

            return result.HasValue && result.Value ? openBrowserDialog.SelectedPath : string.Empty;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return SnackbarMessageQueue;
        }
    }
}