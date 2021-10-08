using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Halloween.DevMasks;
using SpiritMod.NPCs.Critters;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Reach;
using SpiritMod.NPCs.BlueMoon.LunarSlime;
using SpiritMod.NPCs.OceanSlime;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Sword;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;

namespace SpiritMod.Items
{
	public class GItem : GlobalItem
	{
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;

		//bool CandyToolTip = false;

		private GlyphType glyph = 0;
		public GlyphType Glyph => glyph;
		public override void SetDefaults(Item item)
		{
			if (item.type == ItemID.ArmoredCavefish) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.Damselfish) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.CrimsonTigerfish) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.GoldenCarp) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.SpecularFish) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.Prismite) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.Ebonkoi) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.NeonTetra) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.AtlanticCod) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.FrostMinnow) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
			if (item.type == ItemID.RedSnapper) {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = item.useAnimation = 20;
				item.noMelee = true;
				item.consumable = true;
				item.autoReuse = true;
			}
            if (item.type == ItemID.VariegatedLardfish)
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.useTime = item.useAnimation = 20;
                item.noMelee = true;
                item.consumable = true;
                item.autoReuse = true;
            }
        }
		public void SetGlyph(Item item, GlyphType glyph)
		{
			if (this.glyph == glyph)
				return;
			AdjustStats(item, true);
			this.glyph = glyph;
			AdjustStats(item);
		}

		public override void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
			if (item.type == ItemID.RoyalGel) {
				player.npcTypeNoAggro[ModContent.NPCType<LunarSlime>()] = true;
				player.npcTypeNoAggro[ModContent.NPCType<ReachSlime>()] = true;
				player.npcTypeNoAggro[ModContent.NPCType<NPCs.GraniteSlime.GraniteSlime>()] = true;
				player.npcTypeNoAggro[ModContent.NPCType<NPCs.DiseasedSlime.DiseasedSlime>()] = true;
				player.npcTypeNoAggro[ModContent.NPCType<OceanSlime>()] = true;
			}
		}

		private void AdjustStats(Item item, bool remove = false)
		{
			Item norm = new Item();
			norm.netDefaults(item.netID);

			float damage = 0;
			int crit = 0;
			float mana = 0;
			float knockBack = 0;
			float velocity = 0;
			float useTime = 0;
			float size = 0;
			int tileBoost = 0;

			switch (glyph) {
				case GlyphType.Blaze:
					velocity += 1;
					damage += 0.03f;
					break;
				case GlyphType.Phase:
					crit += 7;
					break;
				case GlyphType.Veil:
					useTime -= 0.05f;
					break;
				case GlyphType.Radiant:
					crit += 4;
					break;
				case GlyphType.Efficiency:
					useTime -= 0.3f;
					tileBoost += 2;
					break;
			}

			int s = remove ? -1 : 1;
			item.damage += s * (int)Math.Round(norm.damage * damage);
			item.useAnimation += s * (int)Math.Round(norm.useAnimation * useTime);
			item.useTime += s * (int)Math.Round(norm.useTime * useTime);
			item.reuseDelay += s * (int)Math.Round(norm.reuseDelay * useTime);
			item.mana += s * (int)Math.Round(norm.mana * mana);
			item.knockBack += s * norm.knockBack * knockBack;
			item.scale += s * norm.scale * size;
			if (item.shoot >= ProjectileID.None && !item.melee) //Don't change velocity for spears
			{
				item.shootSpeed += s * norm.shootSpeed * velocity;
			}
			item.crit += s * crit;
			item.tileBoost += s * tileBoost;
			if (remove) {
				if (item.knockBack > norm.knockBack - .0001 &&
					item.knockBack < norm.knockBack + .0001)
					item.knockBack = norm.knockBack;
				if (item.scale > norm.scale - .0001 &&
					item.scale < norm.scale + .0001)
					item.scale = norm.scale;
				if (item.shootSpeed > norm.shootSpeed - .0001 &&
					item.shootSpeed < norm.shootSpeed + .0001)
					item.shootSpeed = norm.shootSpeed;
			}
		}

		public override bool NeedsSaving(Item item) => glyph != GlyphType.None;

		public override TagCompound Save(Item item)
		{
			TagCompound data = new TagCompound();
			data.Add("glyph", (int)glyph);
			return data;
		}

		public override void Load(Item item, TagCompound data)
		{
			GlyphType glyph = (GlyphType)data.GetInt("glyph");
			if (glyph > GlyphType.None && glyph < GlyphType.Count)
				this.glyph = glyph;
			else
				this.glyph = GlyphType.None;
			AdjustStats(item);
		}

		public override void NetSend(Item item, BinaryWriter writer) => writer.Write((byte)glyph);

		public override void NetReceive(Item item, BinaryReader reader)
		{
			GlyphType glyph = (GlyphType)reader.ReadByte();
			if (glyph > GlyphType.None && glyph < GlyphType.Count)
				this.glyph = glyph;
			else
				this.glyph = GlyphType.None;
			AdjustStats(item);
		}

		public override void PostReforge(Item item) => AdjustStats(item);

		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context != "goodieBag")
				return;
			ItemUtils.DropCandy(player);
			if (Main.rand.Next(3) == 0) {
				int[] lootTable = {
					ModContent.ItemType<MaskSchmo>(),
					ModContent.ItemType<MaskGraydee>(),
					ModContent.ItemType<MaskLordCake>(),
					ModContent.ItemType<MaskVladimier>(),
					ModContent.ItemType<MaskKachow>(),
					ModContent.ItemType<MaskBlaze>(),
					ModContent.ItemType<MaskSvante>(),
					ModContent.ItemType<MaskIggy>(),
					ModContent.ItemType<MaskYuyutsu>(),
					ModContent.ItemType<MaskLeemyy>()
				};
				int loot = Main.rand.Next(lootTable.Length);
				player.QuickSpawnItem(lootTable[loot]);
			}
		}

		public override float UseTimeMultiplier(Item item, Player player)
		{
			float speed = 1f;
			if (player.GetModPlayer<MyPlayer>().blazeBurn)
				speed += .17f;
			if (player.GetModPlayer<MyPlayer>().leatherGlove)
				speed += .06f;
			if (player.GetModPlayer<MyPlayer>().frigidGloves)
				speed += .04f * player.GetModPlayer<MyPlayer>().frigidGloveStacks;
			return speed;
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
		{
			MyPlayer spirit = player.GetModPlayer<MyPlayer>();
			if (glyph == GlyphType.Phase) {
				float boost = 0.005f * spirit.SpeedMPH;
				if (boost > 0.5f)
					boost = 0.5f;
				mult = 1 + 1 * boost;
			}
			if (item.summon && spirit.silkenSet) {
				flat += 1;
			}
		}

		public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (glyph == GlyphType.Unholy)
				Glyphs.UnholyGlyph.PlagueEffects(target, player.whoAmI, ref damage, crit);
			else if (glyph == GlyphType.Phase)
				Glyphs.PhaseGlyph.PhaseEffects(player, ref damage, crit);
			else if (glyph == GlyphType.Daze)
				Glyphs.DazeGlyph.Daze(target, ref damage);
			else if (glyph == GlyphType.Radiant)
				Glyphs.RadiantGlyph.DivineStrike(player, ref damage);
		}

		public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
		{
			switch (glyph) {
				case GlyphType.Frost:
					Glyphs.FrostGlyph.CreateIceSpikes(player, target, crit);
					break;
				case GlyphType.Sanguine:
					Glyphs.SanguineGlyph.BloodCorruption(player, target, damage);
					break;
				case GlyphType.Blaze:
					Glyphs.BlazeGlyph.Rage(player, target);
					break;
				case GlyphType.Bee:
					Glyphs.BeeGlyph.ReleaseBees(player, target, damage);
					break;
			}
		}

		public override void ModifyHitPvp(Item item, Player player, Player target, ref int damage, ref bool crit)
		{
			if (glyph == GlyphType.Phase)
				Glyphs.PhaseGlyph.PhaseEffects(player, ref damage, crit);
			else if (glyph == GlyphType.Daze)
				Glyphs.DazeGlyph.Daze(target, ref damage);
			else if (glyph == GlyphType.Radiant)
				Glyphs.RadiantGlyph.DivineStrike(player, ref damage);
		}

		public override void OnHitPvp(Item item, Player player, Player target, int damage, bool crit)
		{
			switch (glyph) {
				case GlyphType.Sanguine:
					Glyphs.SanguineGlyph.BloodCorruption(player, target, damage);
					break;
				case GlyphType.Blaze:
					Glyphs.BlazeGlyph.Rage(player);
					break;
			}
		}

		public override void UseStyle(Item item, Player player)
		{
			//First frame of useage
			if (player.itemAnimation == player.itemAnimationMax - 1 && (player.reuseDelay > 0 || player.HeldItem.reuseDelay == 0)) {
				if (glyph == GlyphType.Storm) {
					MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
					Glyphs.StormGlyph.WindBurst(modPlayer, item);
				}
			}
		}

		public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.talonSet && (item.ranged || item.magic) && Main.rand.Next(10) == 0) {
				int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY + 2), ProjectileID.HarpyFeather, 10, 2f, player.whoAmI);
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].friendly = true;
			}
			if (modPlayer.cultistScarf && item.magic && Main.rand.Next(8) == 0) {
				int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<WildMagic>(), 66, 2f, player.whoAmI);
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].friendly = true;
			}
			if (modPlayer.timScroll && item.magic && Main.rand.Next(12) == 0) {
				int p = Main.rand.Next(9, 23);
				if (p != 11 && p != 13 && p != 18 && p != 17 && p != 21) {
					int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, p, damage, knockBack, player.whoAmI, 0f, 0f);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
				}
			}
			if (modPlayer.crystal && item.ranged && Main.rand.Next(8) == 0) {
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX * (float)(Main.rand.Next(100, 165) / 100), speedY * (float)(Main.rand.Next(100, 165) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
			}

			if (modPlayer.fireMaw && item.magic && Main.rand.Next(4) == 0) {
				int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<FireMaw>(), 30, 2f, player.whoAmI);
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].friendly = true;
			}
			if (modPlayer.manaWings && item.magic && Main.rand.Next(7) == 0) {
				float d1 = 20 + ((player.statManaMax2 - player.statMana) / 3);
				int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<ManaSpark>(), (int)d1, 2f, player.whoAmI);
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].friendly = true;
			}

			return true;
		}
		public override void GrabRange(Item item, Player player, ref int grabRange)
		{
			int[] metalItems = new int[] { ItemID.IronOre, ItemID.CopperOre, ItemID.SilverOre, ItemID.GoldOre, ItemID.TinOre, ItemID.LeadOre, ItemID.TungstenOre, ItemID.PlatinumOre, ItemID.Meteorite, ItemID.LunarOre, ItemID.ChlorophyteOre, 
				ItemID.DemoniteOre, ItemID.CrimtaneOre, ItemID.Obsidian, ItemID.Hellstone, ItemID.CobaltOre, ItemID.PalladiumOre, ItemID.MythrilOre, ItemID.OrichalcumOre, ItemID.AdamantiteOre, ItemID.TitaniumOre };
			if (player.GetModPlayer<MyPlayer>().MetalBand)
				if (metalItems.Contains(item.type))
					grabRange *= 10;
			if (player.GetModPlayer<MyPlayer>().teslaCoil)
				if (metalItems.Contains(item.type))
					grabRange *= 15;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			bool insertStats = false;
			if (glyph != GlyphType.None) {
				insertStats = true;

				var lookup = Glyphs.GlyphBase.FromType(glyph);
				if (lookup.Effect != null && lookup.Addendum != null) {
					TooltipLine tip = new TooltipLine(mod, "Glyph", lookup.Effect);
					tip.overrideColor = lookup.Color;
					tooltips.Add(tip);
					tip = new TooltipLine(mod, "GlyphAddendum", lookup.Addendum);
					tooltips.Add(tip);
				}
			}

			if (insertStats && item.prefix <= 0)
				InsertStatInfo(item, tooltips);
		}

		private void InsertStatInfo(Item item, List<TooltipLine> tooltips)
		{
			int index = 0;
			for (int i = tooltips.Count - 1; i >= 0; i--) {
				TooltipLine curr = tooltips[i];
				if (curr.mod != "Terraria")
					continue;
				if (curr.Name == "Price" ||
					curr.Name == "Expert" ||
					curr.Name == "SetBonus")
					continue;

				index = i + 1;
				break;
			}

			Item compare = new Item();
			compare.netDefaults(item.netID);
			string line;
			TooltipLine tip;

			if (compare.damage != item.damage) {
				double damage = (double)((float)item.damage - (float)compare.damage);
				damage = damage / (double)((float)compare.damage) * 100.0;
				damage = Math.Round(damage);
				if (damage > 0.0)
					line = "+" + damage + Language.GetTextValue("LegacyTooltip.39");
				else
					line = damage.ToString() + Language.GetTextValue("LegacyTooltip.39");

				tip = new TooltipLine(mod, "PrefixDamage", line);
				if (damage < 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}

			if (compare.useAnimation != item.useAnimation) {
				double speed = (double)((float)item.useAnimation - (float)compare.useAnimation);
				speed = speed / (double)((float)compare.useAnimation) * 100.0;
				speed = Math.Round(speed);
				speed *= -1.0;
				if (speed > 0.0)
					line = "+" + speed + Language.GetTextValue("LegacyTooltip.40");
				else
					line = speed.ToString() + Language.GetTextValue("LegacyTooltip.40");

				tip = new TooltipLine(mod, "PrefixSpeed", line);
				if (speed < 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}

			if (compare.crit != item.crit) {
				double crit = (double)((float)item.crit - (float)compare.crit);
				if (crit > 0.0)
					line = "+" + crit + Language.GetTextValue("LegacyTooltip.41");
				else
					line = crit.ToString() + Language.GetTextValue("LegacyTooltip.41");

				tip = new TooltipLine(mod, "PrefixCritChance", line);
				if (crit < 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}

			if (compare.mana != item.mana) {
				double mana = (double)((float)item.mana - (float)compare.mana);
				mana = mana / (double)((float)compare.mana) * 100.0;
				mana = Math.Round(mana);
				if (mana > 0.0)
					line = "+" + mana + Language.GetTextValue("LegacyTooltip.42");
				else
					line = mana.ToString() + Language.GetTextValue("LegacyTooltip.42");

				tip = new TooltipLine(mod, "PrefixUseMana", line);
				if (mana > 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}

			if (compare.scale != item.scale) {
				double scale = (double)(item.scale - compare.scale);
				scale = scale / (double)compare.scale * 100.0;
				scale = Math.Round(scale);
				if (scale > 0.0)
					line = "+" + scale + Language.GetTextValue("LegacyTooltip.43");
				else
					line = scale.ToString() + Language.GetTextValue("LegacyTooltip.43");

				tip = new TooltipLine(mod, "PrefixSize", line);
				if (scale < 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}

			if (compare.shootSpeed != item.shootSpeed) {
				double velocity = (double)(item.shootSpeed - compare.shootSpeed);
				velocity = velocity / (double)compare.shootSpeed * 100.0;
				velocity = Math.Round(velocity);
				if (velocity > 0.0)
					line = "+" + velocity + Language.GetTextValue("LegacyTooltip.44");
				else
					line = velocity.ToString() + Language.GetTextValue("LegacyTooltip.44");

				tip = new TooltipLine(mod, "PrefixShootSpeed", line);
				if (velocity < 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}

			if (compare.knockBack != item.knockBack) {
				double knockback = (double)(item.knockBack - compare.knockBack);
				knockback = knockback / (double)compare.knockBack * 100.0;
				knockback = Math.Round(knockback);
				if (knockback > 0.0)
					line = "+" + knockback + Language.GetTextValue("LegacyTooltip.45");
				else
					line = knockback.ToString() + Language.GetTextValue("LegacyTooltip.45");

				tip = new TooltipLine(mod, "PrefixKnockback", line);
				if (knockback < 0.0)
					tip.isModifierBad = true;

				tip.isModifier = true;
				tooltips.Insert(index++, tip);
			}
		}
		public override bool UseItem(Item item, Player player)
		{
			if (item.type == ItemID.AtlanticCod) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<AtlanticCod>());
				return true;
			}
			if (item.type == ItemID.NeonTetra) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<NeonTetra>());
				return true;
			}
			if (item.type == ItemID.ArmoredCavefish) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Cavefish>());
				return true;
			}
			if (item.type == ItemID.Damselfish) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Damselfish>());
				return true;
			}
			if (item.type == ItemID.CrimsonTigerfish) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<CrismonTigerfish>());
				return true;
			}
			if (item.type == ItemID.GoldenCarp) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<GoldenCarp>());
				return true;
			}
			if (item.type == ItemID.SpecularFish) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<SpecularFish>());
				return true;
			}
			if (item.type == ItemID.Ebonkoi) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Ebonkoi>());
				return true;
			}
			if (item.type == ItemID.Prismite) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Prismite>());
				return true;
			}
			if (item.type == ItemID.FrostMinnow) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<FrostMinnow>());
				return true;
			}
			if (item.type == ItemID.RedSnapper) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<RedSnapper>());
				return true;
			}
			if (item.type == ItemID.VariegatedLardfish) {
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Lardfish>());
				return true;
			}
			else {
				return false;
			}
		}

		private static readonly Vector2 SlotDimensions = new Vector2(52, 52);
		public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (glyph == GlyphType.None)
				return;

			float slotScale = 1f;
			if (frame.Width > 32 || frame.Height > 32) {
				if (frame.Width > frame.Height)
					slotScale = 32f / frame.Width;
				else
					slotScale = 32f / frame.Height;
			}
			slotScale *= Main.inventoryScale;
			Vector2 slotOrigin = position + frame.Size() * (.5f * slotScale);
			slotOrigin -= SlotDimensions * (.5f * Main.inventoryScale);

			Texture2D texture = Glyphs.GlyphBase.FromType(glyph).Overlay;
			if (texture != null) {
				Vector2 offset = SlotDimensions;
				offset -= texture.Size();
				offset -= new Vector2(4f);
				offset *= Main.inventoryScale;
				//offset += new Vector2(40f, 40f) * Main.inventoryScale;//stack offset Vector2(10f, 26f)
				spriteBatch.Draw(texture, slotOrigin + offset, null, drawColor, 0f, Vector2.Zero, Main.inventoryScale, SpriteEffects.None, 0f);
			}
		}

		public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Color glowColor = new Color(250, 250, 250, item.alpha);
			IGlowing glow = item.modItem as IGlowing;
			if (glow != null) {
				float bias = 0f;
				Texture2D texture = glow.Glowmask(out bias);
				Color alpha = Color.Lerp(alphaColor, glowColor, bias);
				Vector2 origin = new Vector2(texture.Width >> 1, texture.Height >> 1);
				Vector2 position = item.position - Main.screenPosition;
				position.X += item.width >> 1;
				position.Y += item.height - (texture.Height >> 1) + 2f;
				spriteBatch.Draw(texture, position, null, alpha, rotation, origin, scale, SpriteEffects.None, 0f);
			}

			if (glyph != GlyphType.None) {
				Texture2D texture = Glyphs.GlyphBase.FromType(glyph).Overlay;
				if (texture != null) {
					Vector2 position = item.position - Main.screenPosition;
					position.X += (item.width >> 1);
					position.Y += 2 + item.height - (Main.itemTexture[item.type].Height >> 1);

					//Color alpha = Color.Lerp(alphaColor, glowColor, .2f);
					Color alpha = alphaColor;
					alpha.R = (byte)Math.Min(alpha.R + 25, 255);
					alpha.G = (byte)Math.Min(alpha.G + 25, 255);
					alpha.B = (byte)Math.Min(alpha.B + 25, 255);
					//alpha.A = (byte)(alpha.A * .6f);
					Vector2 origin = new Vector2(texture.Width >> 1, texture.Height >> 1);
					spriteBatch.Draw(texture, position, null, alpha, rotation, origin, scale, SpriteEffects.None, 0f);
				}
			}
		}
	}
}
