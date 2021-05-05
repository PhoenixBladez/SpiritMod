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

namespace SpiritMod.Mechanics.QuestSystem
{
	public class InstancedQuest : Quest
	{
		private string _questName;
		private int _questDifficulty;
		private string _questDescription;
		private string _questClient;
		private (int, int)[] _questRewards;

		public override string QuestName => _questName;
		public override int Difficulty => _questDifficulty;
		public override QuestType QuestType => QuestType.Other;
		public override string QuestDescription => _questDescription;
		public override string QuestClient => _questClient;
		public override (int, int)[] QuestRewards => _questRewards;

		public InstancedQuest(string name, int difficulty, string client, string description, (int, int)[] rewards, Texture2D image, List<IQuestTask> tasks)
		{
			_questName = name;
			_questDifficulty = difficulty;
			_questClient = client;
			_questDescription = description;
			_questRewards = rewards;
			QuestImage = image;
			_questTasks = tasks;
		}
	}
}
