﻿#pragma checksum "C:\Users\DeadMan\Desktop\School\10_Fall_2024\ProgramConstruction1\Assignments\Assignment-05\TeamGGalaga\Galaga\View\GameCanvas.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "492C2402C056E4173EE8BFEC410291FE7220DDBADD49A8EDE397A52671D8A16E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galaga.View
{
    partial class GameCanvas : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // View\GameCanvas.xaml line 10
                {
                    this.canvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 3: // View\GameCanvas.xaml line 11
                {
                    this.scoreBoard = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // View\GameCanvas.xaml line 13
                {
                    this.playerLivesBoard = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // View\GameCanvas.xaml line 15
                {
                    this.gameOverYouLWin = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6: // View\GameCanvas.xaml line 17
                {
                    this.gameOverYouLose = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
