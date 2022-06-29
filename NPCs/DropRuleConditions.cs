using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs
{
	public class DropRuleConditions
	{
		public class NotDay : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!info.IsInSimulation)
					return !Main.dayTime;
				return false;
			}

			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => "Drops only at night";
		}

		public class Day : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!info.IsInSimulation)
					return Main.dayTime;
				return false;
			}

			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => "Drops only during day";
		}

		public class BossDowned : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public enum Bosses : int
			{
				Evil_Boss,
				Any_Mech,
				King_Slime,
				Scarabeus,
				Skeletron,
				SingularCutoff, //Keep this after all non-"the" bosses (i.e. after Skeletron but before THE Eye of Cthulhu)
				Eye_Of_Cthulhu,
				Moon_Jelly_Wizard,
				Queen_Bee,
			}

			readonly Bosses boss = Bosses.King_Slime;

			public BossDowned(Bosses boss)
			{
				this.boss = boss;
			}

			public bool CanDrop(DropAttemptInfo info)
			{
				if (!info.IsInSimulation)
				{
					return boss switch
					{
						Bosses.King_Slime => NPC.downedSlimeKing,
						Bosses.Scarabeus => MyWorld.downedScarabeus,
						Bosses.Eye_Of_Cthulhu => NPC.downedBoss1,
						Bosses.Evil_Boss => NPC.downedBoss2,
						Bosses.Skeletron => NPC.downedBoss3,
						Bosses.Queen_Bee => NPC.downedQueenBee,
						Bosses.Moon_Jelly_Wizard => MyWorld.downedMoonWizard,
						Bosses.Any_Mech => NPC.downedMechBossAny,
						_ => false,
					};
				}
				return false;
			}

			public bool CanShowItemDropInUI() => true;

			public string GetConditionDescription()
			{
				string def = "Drops only after {X} has been defeated";
				if (boss != Bosses.Evil_Boss && boss != Bosses.Any_Mech)
				{
					string plural = (int)boss < (int)Bosses.SingularCutoff ? "" : "the ";
					def = def.Replace("{X}", plural + boss.ToString().Replace("_", " "));
				}
				else if (boss == Bosses.Evil_Boss)
					def = def.Replace("{X}", WorldGen.crimson ? "the Brain Of Cthulhu" : "the Eater of Worlds");
				else if (boss == Bosses.Any_Mech)
					def = def.Replace("{X}", "any mechanical boss");
				return def;
			}
		}

		public class InBiome : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public enum Biome : int
			{
				AnyPurity,
				SurfacePurity,
				UndergroundPurity,
				Snow,
				Asteroid
			}

			readonly Biome biome = Biome.AnyPurity;

			public InBiome(Biome biome)
			{
				this.biome = biome;
			}

			public bool CanDrop(DropAttemptInfo info)
			{
				if (!info.IsInSimulation)
				{
					return biome switch
					{
						Biome.AnyPurity => info.player.ZonePurity,
						Biome.SurfacePurity => info.player.ZonePurity && info.player.ZoneOverworldHeight,
						Biome.UndergroundPurity => info.player.ZonePurity && info.player.ZoneDirtLayerHeight,
						Biome.Asteroid => info.player.GetModPlayer<MyPlayer>().ZoneAsteroid,
						Biome.Snow => info.player.ZoneSnow,
						_ => false,
					};
				}
				return false;
			}

			public bool CanShowItemDropInUI() => true;

			public string GetConditionDescription()
			{
				string def = "Drops only in the ";
				if (biome.ToString().Contains("Purity")) 
					return def + "Purity";
				else
					return def + biome.ToString();
			}
		}

		public class PlayerConditional : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!info.IsInSimulation)
					return canDrop(info.player);
				return false;
			}

			public Func<Player, bool> canDrop;
			public readonly string condition = "";

			public PlayerConditional(string cond, Func<Player, bool> func)
			{
				condition = cond;
				canDrop = func;
			}

			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => condition;
		}
	}
}
