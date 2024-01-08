// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

namespace Utage
{

	//新しいプロジェクトを作る処理
	[Serializable]
	public abstract class AdvProjectCreator 
		: IAdvProjectCreator
	{
		public string ProjectName { get; private set; }

		public string NewProjectDir { get; private set; }

		public string TemplateFolderPath { get; private set; }
		public string TemplateFolderName { get; private set; }

		public AdvProjectTemplateSettings TemplateSettings { get; private set; }
		
		//新規プロジェクト作成可能か
		public abstract bool EnableCreate();


		protected AdvProjectCreator(AdvProjectTemplateSettings templateSettings)
		{
			TemplateSettings = templateSettings;
			TemplateFolderPath = AssetDatabase.GetAssetPath(TemplateSettings.TemplateFolder);
			TemplateFolderName = Path.GetFileName(TemplateFolderPath);
		}

		//新たなプロジェクトを作成
		public virtual void Create(string projectName)
		{
			SetProjectName(projectName);
			OnCreate();
		}

		protected abstract void OnCreate();

		protected void SetProjectName(string projectName)
		{
			ProjectName = projectName;
			NewProjectDir = Path.Combine(Application.dataPath, ProjectName) + "/";
		}

		public string GetProjectRelativeDir()
		{
			return Path.Combine("Assets", ProjectName);
		}

		public string GetRelativeProjectNameFilePath(string ext)
		{
			return Path.Combine(GetProjectRelativeDir(), ProjectName + ext);
		}
	}
}
