using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class ConsoleHelpWriter : TextWriterHelpWriter
    {
        private enum ConsoleStatus
        {
            None,
            Normal,
            Warning,
            Error
        }

        public ConsoleHelpWriter()
            : base(Console.Out)
        {
        }

        /// <summary>
        /// <see cref="Status" /> のバッキングストアです。
        /// </summary>
        private ConsoleStatus _Status;

        public ConsoleColor? WarningBackgroundColor { get; set; }

        public ConsoleColor? WarningForegroundColor { get; set; } = ConsoleColor.Yellow;

        public ConsoleColor? ErrorBackgroundColor { get; set; } = ConsoleColor.Red;

        public ConsoleColor? ErrorForegroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// TODO:プロパティの名前を取得または設定します。
        /// </summary>
        private ConsoleStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (value != _Status)
                {
                    _Status = value;

                    ConsoleColor? bg = null, fg = null;
                    switch (value)
                    {
                        case ConsoleStatus.Warning:
                            bg = WarningBackgroundColor;
                            fg = WarningForegroundColor;
                            break;

                        case ConsoleStatus.Error:
                            bg = ErrorBackgroundColor;
                            fg = ErrorForegroundColor;
                            break;

                        default:
                            break;
                    }

                    Console.BackgroundColor = bg ?? Console.BackgroundColor;
                    Console.ForegroundColor = fg ?? Console.ForegroundColor;
                }
            }
        }

        public override void ResetMessageType()
            => Status = ConsoleStatus.Normal;

        public override void EnterWarning()
            => Status = ConsoleStatus.Warning;

        public override void EnterError()
            => Status = ConsoleStatus.Error;

        public override void WriteCode(string message)
        {
            var bg = Console.BackgroundColor;
            var fg = Console.ForegroundColor;

            Console.BackgroundColor = fg;
            Console.ForegroundColor = bg;

            base.WriteCode(message);

            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
        }
    }
}