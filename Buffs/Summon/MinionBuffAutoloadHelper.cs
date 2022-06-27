using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Buffs.Summon
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AutoloadMinionBuff : Attribute
	{
		public readonly string BuffName;
		public readonly string Description;
		public AutoloadMinionBuff(string BuffName, string Description)
		{
			this.BuffName = BuffName;
			this.Description = Description;
		}
	}

	public class AutoloadMinionPlayer : ModPlayer
	{
		public IDictionary<int, bool> ActiveMinionDict = new Dictionary<int, bool> { }; //type of projectile, then whether or not that minion is active

		public override void Initialize()
		{
			ActiveMinionDict = new Dictionary<int, bool> { };
			foreach (KeyValuePair<int, int> kvp in AutoloadMinionDictionary.BuffDictionary)
				ActiveMinionDict.Add(kvp.Key, false);
		}

		public override void ResetEffects()
		{
			IDictionary<int, bool> dummy = new Dictionary<int, bool>();
			foreach (KeyValuePair<int, bool> kvp in ActiveMinionDict)
				dummy.Add(kvp.Key, false);

			ActiveMinionDict = dummy;
		}

		public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (AutoloadMinionDictionary.BuffDictionary.TryGetValue(type, out int buffType))
				Player.AddBuff(buffType, 180);

			return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}

	public class AutoloadMinionGlobalProjectile : GlobalProjectile
	{
		public override bool PreAI(Projectile projectile) 
		{
			if (AutoloadMinionDictionary.BuffDictionary.ContainsKey(projectile.type))
			{
				Player player = Main.player[projectile.owner];
				AutoloadMinionPlayer modPlayer = player.GetModPlayer<AutoloadMinionPlayer>();
				if (player.dead || !player.active)
					modPlayer.ActiveMinionDict[projectile.type] = false;

				if (modPlayer.ActiveMinionDict[projectile.type])
					projectile.timeLeft = 2;
			}
			return base.PreAI(projectile);
		}
	}

	public static class AutoloadMinionDictionary
	{
		public static IDictionary<int, int> BuffDictionary = new Dictionary<int, int> { }; //type of projectile, then corresponding type of buff

		public static void AddBuffs(Assembly code)
		{
			var autoloadminions = code.GetTypes().Where(x => x.IsSubclassOf(typeof(ModProjectile)) && Attribute.IsDefined(x, typeof(AutoloadMinionBuff))); //read the assembly to find classes that are mod projectiles, and have the autoload minion buff attribute
			foreach(Type MinionType in autoloadminions)
			{
				AutoloadMinionBuff attribute = (AutoloadMinionBuff)Attribute.GetCustomAttribute(MinionType, typeof(AutoloadMinionBuff)); 
				ModProjectile mProjectile = (ModProjectile)Activator.CreateInstance(MinionType);
				SpiritMod.Instance.AddBuff(MinionType.Name + "_buff", new AutoloadedMinionBuff(SpiritMod.Instance.Find<ModProjectile>(MinionType.Name).Type, attribute.BuffName, attribute.Description), MinionType.FullName.Replace(".", "/") + "_buff");
				BuffDictionary.Add(SpiritMod.Instance.Find<ModProjectile>(MinionType.Name).Type, SpiritMod.Instance.Find<ModBuff>(MinionType.Name + "_buff").Type);
			}
		}

		public static void Unload() => BuffDictionary.Clear();
	}

	public sealed class AutoloadedMinionBuff : ModBuff
	{
		private readonly int MinionType;
		private readonly string BuffName;
		private readonly string BuffDescription;

		public AutoloadedMinionBuff(int MinionType, string BuffName, string BuffDescription)
		{
			this.MinionType = MinionType;
			this.BuffName = BuffName;
			this.BuffDescription = BuffDescription;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(BuffName);
			Description.SetDefault(BuffDescription);
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			AutoloadMinionPlayer modPlayer = player.GetModPlayer<AutoloadMinionPlayer>();
			if (player.ownedProjectileCounts[MinionType] > 0)
			{
				modPlayer.ActiveMinionDict[MinionType] = true;
			}

			if (!modPlayer.ActiveMinionDict[MinionType])
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 180;
		}
	}
}
