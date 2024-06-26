﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.Models.Interfaces;

public interface IProjectConfig
{
    public string ProjectPath { get; }
    public string ProjectConfigPath { get; }
    public string ProjectName { get; set; }

    public ref ObservableCollection<string> MarkupClasses { get; }
    public IMarkupFormatter DataFormat { get; set; }
    public ObservableCollection<string> ExcludedImages { get; }

    public virtual void addMarkupClass(string markupClassName) { }
    public virtual void removeMarkupClass(string markupClassName) { }
    public virtual void renameMarkupClassTo(string markupClassName, string newMarkupClassName) { }

    public virtual void excludeImage(string ImagePath) { }
    public virtual void includeImage(string ImagePath) { }
}
