using System;

using Terraria;
using Terraria.ID;
using static Terraria.ID.BuffID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SpiritMod.Items.Halloween
{
	public class MysteryCandy : CandyBase
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystery Candy");
			Tooltip.SetDefault("Either a great treat... or a nasty trick");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = 2;
			item.maxStack = 30;

			item.useStyle = 2;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = mod.BuffType("MysteryBuff");
			item.buffTime = 14400;

			item.UseSound = SoundID.Item2;
		}

		public override bool UseItem(Player player)
		{
			string line = "";
			Color color = new Color(127, 255, 127);
			if (Main.rand.Next(100) < 66)
			{
				//Positive effect
				switch (Main.rand.Next(14))
				{
					case 0:
						line = "Sweet and filling. You feel restored.";
						player.AddBuff(Lifeforce, 36000);
						player.AddBuff(Regeneration, 36000);
						player.AddBuff(RapidHealing, 1800);
						break;
					case 1:
						line = "The sugar rush hits hard.";
						player.AddBuff(Swiftness, 36000);
						player.AddBuff(Panic, 10800);
						break;
					case 2:
						line = "Mmm, the fluffiest of nougats. You feel light as a feather.";
						player.AddBuff(Featherfall, 36000);
						player.AddBuff(WaterWalking, 36000);
						//player.AddBuff(Gills, 36000);
						break;
					case 3:
						line = "Magic is sparking from your fingertips. Maybe the candy was enchanted after all.";
						player.AddBuff(Clairvoyance, 36000);
						player.AddBuff(MagicPower, 36000);
						player.AddBuff(ManaRegeneration, 36000);
						break;
					case 4:
						line = "Has a bit of a metallic taste, but your skin is now tough as titanium.";
						player.AddBuff(Ironskin, 36000);
						player.AddBuff(Endurance, 36000);
						break;
					case 5:
						line = "It's a great time to go exploring.";
						player.AddBuff(Mining, 36000);
						player.AddBuff(Shine, 36000);
						break;
					case 6:
						line = "You can see all.";
						player.AddBuff(Spelunker, 36000);
						player.AddBuff(Hunter, 36000);
						player.AddBuff(Dangersense, 36000);
						break;
					case 7:
						line = "Your hands are perfectly steady now.";
						player.AddBuff(Archery, 36000);
						player.AddBuff(AmmoReservation, 36000);
						break;
					case 8:
						line = "That was delicious.";
						if (player.statLife < player.statLifeMax)
						{
							player.HealEffect(Math.Min(200, player.statLifeMax - player.statLife));
							player.statLife += 200;
						}
						player.AddBuff(WellFed, 36000);
						player.AddBuff(Honey, 1800);
						break;
					case 9:
						line = "The conditions are perfect for a bit of building.";
						player.AddBuff(Calm, 36000);
						player.AddBuff(Builder, 36000);
						break;
					case 10:
						line = "Your wish is their command.";
						player.AddBuff(Summoning, 86400);
						player.AddBuff(Bewitched, 86400);
						break;
					case 11:
						line = "New strength surges through your body.";
						player.AddBuff(Rage, 36000);
						player.AddBuff(Wrath, 36000);
						break;
					case 12:
						line = "The ground drops beneath your feet.";
						player.gravDir *= -1;
						player.velocity.Y += player.gravDir * 5;
						player.AddBuff(Gravitation, 18000);
						//player.AddBuff(VortexDebuff, 1800);
						break;
					case 13:
						line = "You feel like you're gonna make a big catch.";
						player.AddBuff(Fishing, 54000);
						player.AddBuff(Sonar, 54000);
						player.AddBuff(Crate, 54000);
						break;
				}
			} else {
				color = new Color(255, 127, 127);
				switch (Main.rand.Next(11))
				{
					case 0:
						player.Hurt(PlayerDeathReason.ByCustomReason(player.name +" bit on a razorblade."), (int)(player.statLifeMax * .25f), 0);
						if (player.statLife > 0)
							line = "Ouch, there was a razorblade in there!";
						player.AddBuff(Bleeding, 3600);
						break;
					case 1:
						line = "Your legs can barely hold you.";
						player.AddBuff(Slow, 3600);
						player.AddBuff(Dazed, 3600);
						break;
					case 2:
						line = "You are encased in stone.";
						player.AddBuff(Stoned, 300);
						player.AddBuff(Suffocation, 300);
						player.AddBuff(OgreSpit, 3600);
						break;
					case 3:
						line = "Every last spark of magic left your body. Maybe the candy was enchanted after all.";
						player.statMana = 0;
						player.AddBuff(ManaSickness, 1200);
						player.AddBuff(Silenced, 3600);
						break;
					case 4:
						line = "Your armor is crumpling at the slightest touch.";
						player.AddBuff(BrokenArmor, 7200);
						player.AddBuff(WitheredArmor, 7200);
						player.AddBuff(Ichor, 7200);
						break;
					case 5:
						line = "You can barely think straight.";
						player.AddBuff(Obstructed, 1200);
						player.AddBuff(Blackout, 1200);
						player.AddBuff(Cursed, 7200);
						break;
					case 6:
						line = "You are frozen in place. Maybe mint was a bad idea...";
						player.AddBuff(Frozen, 600);
						player.AddBuff(Frostburn, 600);
						player.AddBuff(Chilled, 3600);
						break;
					case 7:
						line = "You can't feel your arms.";
						player.AddBuff(WitheredWeapon, 7200);
						player.AddBuff(Weak, 7200);
						break;
					case 8:
						line = "Your limbs ache with the slightest movement.";
						player.AddBuff(Electrified, 1500);
						player.AddBuff(Webbed, 300);
						break;
					case 9:
						line = "You've been drugged.";
						player.AddBuff(Darkness, 7200);
						player.AddBuff(Confused, 7200);
						player.AddBuff(Tipsy, 7200);
						player.AddBuff(Titan, 7200);
						break;
					case 10:
						line = "You feel sick.";
						player.AddBuff(Bleeding, 3600);
						player.AddBuff(PotionSickness, 10800);
						break;
				}
			}
			if (line != "")
				Main.NewText(line, color.R, color.G, color.B);
			
			return true;
		}
	}
}
