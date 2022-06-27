using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace SpiritMod.Items.Halloween
{
	public class MysteryCandy : CandyBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystery Candy");
			Tooltip.SetDefault("Either a great treat... or a nasty trick");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 30;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.UseSound = SoundID.Item2;
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			string line = "";
			Color color = new Color(127, 255, 127);
			if (Main.rand.Next(100) < 66) {
				//Positive effect
				switch (Main.rand.Next(14)) {
					case 0:
						line = "Sweet and filling. You feel restored.";
						player.AddBuff(BuffID.Lifeforce, 36000);
						player.AddBuff(BuffID.Regeneration, 36000);
						player.AddBuff(BuffID.RapidHealing, 1800);
						break;
					case 1:
						line = "The sugar rush hits hard.";
						player.AddBuff(BuffID.Swiftness, 36000);
						player.AddBuff(BuffID.Panic, 10800);
						break;
					case 2:
						line = "Mmm, the fluffiest of nougats. You feel light as a feather.";
						player.AddBuff(BuffID.Featherfall, 36000);
						player.AddBuff(BuffID.WaterWalking, 36000);
						//player.AddBuff(Gills, 36000);
						break;
					case 3:
						line = "Magic is sparking from your fingertips. Maybe the candy was enchanted after all.";
						player.AddBuff(BuffID.Clairvoyance, 36000);
						player.AddBuff(BuffID.MagicPower, 36000);
						player.AddBuff(BuffID.ManaRegeneration, 36000);
						break;
					case 4:
						line = "Has a bit of a metallic taste, but your skin is now tough as titanium.";
						player.AddBuff(BuffID.Ironskin, 36000);
						player.AddBuff(BuffID.Endurance, 36000);
						break;
					case 5:
						line = "It's a great time to go exploring.";
						player.AddBuff(BuffID.Mining, 36000);
						player.AddBuff(BuffID.Shine, 36000);
						break;
					case 6:
						line = "You can see all.";
						player.AddBuff(BuffID.Spelunker, 36000);
						player.AddBuff(BuffID.Hunter, 36000);
						player.AddBuff(BuffID.Dangersense, 36000);
						break;
					case 7:
						line = "Your hands are perfectly steady now.";
						player.AddBuff(BuffID.Archery, 36000);
						player.AddBuff(BuffID.AmmoReservation, 36000);
						break;
					case 8:
						line = "That was delicious.";
						if (player.statLife < player.statLifeMax) {
							player.HealEffect(Math.Min(200, player.statLifeMax - player.statLife));
							player.statLife += 200;
						}
						player.AddBuff(BuffID.WellFed, 36000);
						player.AddBuff(BuffID.Honey, 1800);
						break;
					case 9:
						line = "The conditions are perfect for a bit of building.";
						player.AddBuff(BuffID.Calm, 36000);
						player.AddBuff(BuffID.Builder, 36000);
						break;
					case 10:
						line = "Your wish is their command.";
						player.AddBuff(BuffID.Summoning, 86400);
						player.AddBuff(BuffID.Bewitched, 86400);
						break;
					case 11:
						line = "New strength surges through your body.";
						player.AddBuff(BuffID.Rage, 36000);
						player.AddBuff(BuffID.Wrath, 36000);
						break;
					case 12:
						line = "The ground drops beneath your feet.";
						player.gravDir *= -1;
						player.velocity.Y += player.gravDir * 5;
						player.AddBuff(BuffID.Gravitation, 18000);
						//player.AddBuff(VortexDebuff, 1800);
						break;
					case 13:
						line = "You feel like you're gonna make a big catch.";
						player.AddBuff(BuffID.Fishing, 54000);
						player.AddBuff(BuffID.Sonar, 54000);
						player.AddBuff(BuffID.Crate, 54000);
						break;
				}
			}
			else {
				color = new Color(255, 127, 127);
				switch (Main.rand.Next(11)) {
					case 0:
						player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " bit on a razorblade."), (int)(player.statLifeMax * .25f), 0);
						if (player.statLife > 0)
							line = "Ouch, there was a razorblade in there!";
						player.AddBuff(BuffID.Bleeding, 3600);
						break;
					case 1:
						line = "Your legs can barely hold you.";
						player.AddBuff(BuffID.Slow, 3600);
						player.AddBuff(BuffID.Dazed, 3600);
						break;
					case 2:
						line = "You are encased in stone.";
						player.AddBuff(BuffID.Stoned, 300);
						player.AddBuff(BuffID.Suffocation, 300);
						player.AddBuff(BuffID.OgreSpit, 3600);
						break;
					case 3:
						line = "Every last spark of magic left your body. Maybe the candy was enchanted after all.";
						player.statMana = 0;
						player.AddBuff(BuffID.ManaSickness, 1200);
						player.AddBuff(BuffID.Silenced, 3600);
						break;
					case 4:
						line = "Your armor is crumpling at the slightest touch.";
						player.AddBuff(BuffID.BrokenArmor, 7200);
						player.AddBuff(BuffID.WitheredArmor, 7200);
						player.AddBuff(BuffID.Ichor, 7200);
						break;
					case 5:
						line = "You can barely think straight.";
						player.AddBuff(BuffID.Obstructed, 1200);
						player.AddBuff(BuffID.Blackout, 1200);
						player.AddBuff(BuffID.Cursed, 7200);
						break;
					case 6:
						line = "You are frozen in place. Maybe mint was a bad idea...";
						player.AddBuff(BuffID.Frozen, 600);
						player.AddBuff(BuffID.Frostburn, 600);
						player.AddBuff(BuffID.Chilled, 3600);
						break;
					case 7:
						line = "You can't feel your arms.";
						player.AddBuff(BuffID.WitheredWeapon, 7200);
						player.AddBuff(BuffID.Weak, 7200);
						break;
					case 8:
						line = "Your limbs ache with the slightest movement.";
						player.AddBuff(BuffID.Electrified, 1500);
						player.AddBuff(BuffID.Webbed, 300);
						break;
					case 9:
						line = "You've been drugged.";
						player.AddBuff(BuffID.Darkness, 7200);
						player.AddBuff(BuffID.Confused, 7200);
						player.AddBuff(BuffID.Tipsy, 7200);
						player.AddBuff(BuffID.Titan, 7200);
						break;
					case 10:
						line = "You feel sick.";
						player.AddBuff(BuffID.Bleeding, 3600);
						player.AddBuff(BuffID.PotionSickness, 10800);
						break;
				}
			}
			if (line != "")
				Main.NewText(line, color.R, color.G, color.B);

			return true;
		}
	}
}
