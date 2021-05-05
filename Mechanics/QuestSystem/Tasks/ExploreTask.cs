using Microsoft.Xna.Framework;
using SpiritMod.Utilities;
using System;
using System.Text;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class ExploreTask : IQuestTask
	{
		public string ModCallName => "Explore";

		private Func<Player, bool> _exploreFunc;
		private float _requiredDistance;
		private float _distancedTravelled;
		private float _storedDistance;
		private string _areaName;

		/// <param name="areaName">Will be used for the objectives like so: - Explore [areaName] (x%)</param>
		public ExploreTask(Func<Player, bool> exploreFunction, float travelDistance, string areaName)
		{
			_exploreFunc = exploreFunction;
			_requiredDistance = travelDistance;
			_areaName = areaName;
		}

		public IQuestTask Parse(object[] args)
		{
			// get the func
			if (!QuestUtils.TryUnbox(args[1], out Func<Player, bool> func))
			{
				return null;
			}

			// get the distance
			if (!QuestUtils.TryUnbox(args[2], out float distance))
			{
				return null;
			}

			// get the area's name
			if (!QuestUtils.TryUnbox(args[3], out string name))
			{
				return null;
			}

			return new ExploreTask(func, distance, name);
		}

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append("- Explore ");
			builder.Append(_areaName);

			// add a progress bracket at the end like: (x%)
			if (showProgress)
			{
				float travelled = _distancedTravelled > _requiredDistance ? _requiredDistance : _distancedTravelled;
				float progress = travelled / _requiredDistance * 100f;
				builder.Append(" (").Append(progress.ToString("N2")).Append("%").Append(")");
			}

			return builder.ToString();
		}

		public void ResetProgress()
		{
			_distancedTravelled = 0f;
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public bool CheckCompletion()
		{
			if (!Main.dedServ && _exploreFunc(Main.LocalPlayer))
			{
				float distanceMoved = Vector2.Distance(Main.LocalPlayer.oldPosition, Main.LocalPlayer.position);
				
				// TODO: finish MP syncing
				switch (Main.netMode)
				{
					case NetmodeID.SinglePlayer:
						_distancedTravelled += distanceMoved;
						break;
					case NetmodeID.MultiplayerClient:
						_storedDistance += distanceMoved;
						break;
				}
			}
			return _distancedTravelled >= _requiredDistance;
		}

		public void OnMPSyncTick()
		{
			// TODO: send the server our stored distance, then reset to 0
			_storedDistance = 0;
		}
	}
}
