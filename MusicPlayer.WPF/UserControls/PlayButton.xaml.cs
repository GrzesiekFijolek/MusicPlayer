using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for PlayButton.xaml
    /// </summary>
    public partial class PlayButton : UserControl, ICommandSource
    {
        public string IconId
        {
            get => (string)GetValue(iconId);
            set => SetValue(iconId, value);
        }
        /// <summary>
        /// allows to set fontawesome icon on this control
        /// </summary>
        public static readonly DependencyProperty iconId =
            DependencyProperty.Register("IconId", typeof(int), typeof(PlayButton),
                new PropertyMetadata(new PropertyChangedCallback(SetIcon)));

        private static void SetIcon(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as PlayButton).FontAwesome.Icon = (FontAwesome.WPF.FontAwesomeIcon)e.NewValue;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(PlayButton), new UIPropertyMetadata(null));


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(PlayButton), new UIPropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(PlayButton), new UIPropertyMetadata(null));


        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            var command = Command;
            var parameter = CommandParameter;
            var target = CommandTarget;

            var routedCmd = command as RoutedCommand;
            if (routedCmd != null && routedCmd.CanExecute(parameter, target))
            {
                routedCmd.Execute(parameter, target);
            }
            else if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        public PlayButton()
        {
            InitializeComponent();
        }


       
    }
}
