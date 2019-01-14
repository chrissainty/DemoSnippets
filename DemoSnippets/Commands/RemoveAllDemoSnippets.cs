﻿// <copyright file="RemoveAllDemoSnippets.cs" company="Matt Lacey Ltd.">
// Copyright (c) Matt Lacey Ltd. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.Design;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace DemoSnippets.Commands
{
    internal sealed class RemoveAllDemoSnippets : BaseCommand
    {
        public const int CommandId = 0x0200;

        private RemoveAllDemoSnippets(AsyncPackage package, OleMenuCommandService commandService)
            : base(package, commandService)
        {
            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in RemoveAllDemoSnippets's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new RemoveAllDemoSnippets(package, commandService);
        }

        private async void Execute(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(this.Package.DisposalToken);

            await OutputPane.Instance.WriteAsync($"Attempting to remove all demosnippets from the toolbox.");

            await ToolboxInteractionLogic.RemoveAllDemoSnippetsAsync(this.Package.DisposalToken);
        }
    }
}
