using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using SpiritMod.Mechanics.QuestSystem.Tasks;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class InstancedQuest : Quest
	{
		private string _questName;
		private int _questDifficulty;
		private string _questDescription;
		private string _questCategory;
		private string _questClient;
		private (int, int)[] _questRewards;

		public override string QuestName => _questName;
		public override int Difficulty => _questDifficulty;
		public override string QuestCategory => _questCategory;
		public override string QuestDescription => _questDescription;
		public override string QuestClient => _questClient;
		public override (int, int)[] QuestRewards => _questRewards;

		public InstancedQuest(string name, string category, int difficulty, string client, string description, (int, int)[] rewards, Texture2D image, List<QuestTask> tasks) : base()
		{
			_questName = name;
			_questDifficulty = difficulty;
			_questCategory = category;
			_questClient = client;
			_questDescription = description;
			_questRewards = rewards;
			QuestImage = image;

			for (int i = 0; i < tasks.Count; i++)
			{
				_tasks.AddTask(tasks[i]);
			}
		}
	}
}
