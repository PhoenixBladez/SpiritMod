using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mounts;
using SpiritMod.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod
{
    public class MyPlayer : ModPlayer
    {
        public const int CAMO_DELAY = 100;
        public int Soldiers = 0;
        internal static bool swingingCheck;
        internal static Item swingingItem;
        public bool TormentLantern = false;
        public bool QuacklingMinion = false;
        public bool bismiteShield = false;
        public int bismiteShieldStacks;
        public bool VampireCloak = false;
        public bool HealCloak = false;
        public bool SpiritCloak = false;
        public bool firewall = false;
        private int Counter;
        private int timerz;
        public bool caltfist = false;
        public bool ZoneBlueMoon = false;
        private int timer1;
        public bool astralSet = false;
        public bool frigidGloves = false;
        public bool magnifyingGlass = false;
        public bool SoulStone = false;
        public bool geodeSet = false;
        public bool daybloomSet = false;
        public int dazzleStacks;
        public bool ToxicExtract = false;
        public bool scarabCharm = false;
        public bool sunStone = false;
        public bool moonStone = false;
        public bool animusLens = false;
        public bool timScroll = false;
        public bool cultistScarf = false;
        public bool fateToken = false;
        public bool geodeRanged = false;
        public bool atmos = false;
        public bool fireMaw = false;
        public bool deathRose = false;
        public bool anglure = false;
        public bool Fierysoul = false;
        public bool manaWings = false;
        public bool infernalFlame = false;
        public bool floranSet = false;
        public bool rogueSet = false;
        public bool crystal = false;
        public bool eyezorEye = false;
        public bool shamanBand = false;
        public bool ChaosCrystal = false;
        public bool wheezeScale = false;
        public bool briarHeart = false;
        public bool winterbornCharmMage = false;
        public bool sepulchreCharm = false;
        public bool HellGaze = false;
        public bool hungryMinion = false;
        public bool magazine = false;
        public bool EaterSummon = false;
        public bool CreeperSummon = false;
        public bool CrystalShield = false;
        public bool leatherGlove = false;
        public bool moonHeart = false;
        public bool babyClampers = false;
        public bool Phantom = false;
        public bool gremlinTooth = false;
        public bool sacredVine = false;
        public bool BlueDust = false;

        public bool onGround = false;
        public bool moving = false;
        public bool flying = false;
        public bool swimming = false;
        public bool copterBrake = false;
        public bool copterFiring = false;
        public int copterFireFrame = 1000;

        public int beetleStacks = 1;
        public int shootDelay = 0;
        public bool bloodfireShield;
        public int bloodfireShieldStacks;
        public int shootDelay1 = 0;
        public int shootDelay2 = 0;
        public int shootDelay3 = 0;
        public bool unboundSoulMinion = false;
        public bool cragboundMinion = false;
        public bool crawlerockMinion = false;
        public bool pigronMinion = false;
        public bool terror1Summon = false;
        public bool terror2Summon = false;
        public bool terror3Summon = false;
        public bool clatterboneShield = false;
        public bool terror4Summon = false;
        public bool minior = false;

        public bool cthulhuMinion = false;
        public double pressedSpecial;
        float distYT = 0f;
        float distXT = 0f;
        float distY = 0f;
        float distX = 0f;
        public Entity LastEnemyHit = null;
        public bool TiteRing = false;
        public bool NebulaPearl = false;
        public bool CursedPendant = false;
        public bool IchorPendant = false;
        public bool KingRock = false;
        public bool spiritNecklace = false;
        public bool starMap = false;
        private const int saveVersion = 0;
        public bool minionName = false;
        public bool Ward = false;
        public bool Ward1 = false;
        public static bool hasProjectile;
        public bool DoomDestiny = false;
        public int HitNumber;
        public bool ZoneSpirit = false;
        public bool ZoneReach = false;
        public int PutridHits = 0;
        public int Rangedhits = 0;
        public bool flametrail = false;
        public bool icytrail = false;
        public bool silkenSet = false;
        public bool EnchantedPaladinsHammerMinion = false;
        public bool ProbeMinion = false;
        public int frigidGloveStacks;
        public int weaponAnimationCounter;

        public bool gemPickaxe = false;
        public int hexBowAnimationFrame;
        public bool carnivorousPlantMinion = false;
        public bool skeletalonMinion = false;
        public bool babyClamper = false;
        public bool beetleMinion = false;
        public bool steamMinion = false;
        public bool aeonMinion = false;
        public bool lihzahrdMinion = false;
        public bool SnakeMinion = false;
        public bool Ghast = false;
        public bool DungeonSummon = false;
        public bool ReachSummon = false;
        public bool gasopodMinion = false;
        public bool tankMinion = false;
        public bool OG = false;
        public bool lavaRock = false;
        public bool Flayer = false;
        public int soulSiphon;
        public bool maskPet = false;
        public bool phantomPet = false;
        public bool lanternPet = false;
        public bool chitinSet = false;
        public bool thrallPet = false;
        public bool jellyfishPet = false;
        public int clatterStacks;
        public bool starPet = false;
        public bool saucerPet = false;
        public bool bookPet = false;
        public bool SwordPet = false;
        public bool shadowPet = false;

        public float SpeedMPH { get; private set; }
        public DashType ActiveDash { get; private set; }
        public GlyphType glyph;
        public int voidStacks = 1;
        public int camoCounter;
        public int veilCounter;
        public bool blazeBurn;
        public int phaseCounter;
        public int phaseStacks;
        public bool phaseShift;
        private float[] phaseSlice = new float[60];
        public int divineCounter;
        public int divineStacks = 1;
        public int stormStacks;
        public int frostCooldown;
        public float frostRotation;
        public bool frostUpdate;
        public int frostTally;
        public int frostCount;

        // Armor set booleans.
        public bool duskSet;
        public bool runicSet;
        public bool icySet;
        public bool depthSet;
        public bool primalSet;
        public bool spiritSet;
        public bool putridSet;
        public bool illuminantSet;
        public bool duneSet;
        public bool lihzahrdSet;
        public bool acidSet;
        public bool reachSet;
        public bool coralSet;
        public bool leatherSet;
        public bool witherSet;
        public bool titanicSet;
        public bool reaperSet;
        public bool shadowSet;
        public bool oceanSet;
        public bool marbleSet;
        public bool windSet;
        public bool cometSet;
        public bool hellSet;
        public bool bloodfireSet;
        public bool quickSilverSet;
        public bool reaperMask;
        public bool magicshadowSet;
        public bool cryoChestplate;
        public bool cryoSet;
        public bool frigidSet;
        public bool rangedshadowSet;
        public bool graniteSet;
        public bool meleeshadowSet;
        public bool infernalSet;
        public bool crystalSet;
        public bool bloomwindSet;
        public bool fierySet;
        public bool starSet;
        public bool magalaSet;
        public bool thermalSet;
        public bool veinstoneSet;
        public bool clatterboneSet;
        public bool ichorSet1;
        public bool ichorSet2;
        public bool talonSet;
        public bool OverseerCharm = false;
        public bool Bauble = false;

        public bool marbleJustJumped;

        // Accessory booleans.
        public bool OriRing;
        public bool SRingOn;
        public bool goldenApple;
        public bool hpRegenRing;
        public bool bubbleShield;
        public bool icySoul;
        public bool mythrilCharm;
        public bool infernalShield;
        public bool shadowGauntlet;
        public bool amazonCharm;
        public bool KingSlayerFlask;
        public bool Resolve;
        public bool DarkBough;
        public bool hellCharm;
        public bool bloodyBauble;
        public bool twilightTalisman;
        public bool MoonSongBlossom;
        public bool HolyGrail;
        public bool bismiteSet;
        public float virulence = 600f;
        public float cryoTimer = 0f;
        public float marbleJump = 420f;
        public bool moonGauntlet;
        public bool starCharm;
        public int timeLeft = 0;
        public int infernalHit;
        public int infernalDash;

        public bool windEffect;
        public bool windEffect2;
        public int infernalSetCooldown;
        public int fierySetTimer = 480;
        public int firewallHit;
        public int bubbleTimer;
        public int clatterboneTimer;
        public int roseTimer;
        public int baubleTimer;
        public int cometTimer;
        public bool concentrated; // For the leather armor set.
        public int concentratedCooldown = 360;
        public int stompCooldown = 600;
        public bool basiliskMount;
        public bool drakomireMount;
        public int drakomireFlameTimer;
        public bool drakinMount;
        public bool toxify;
        public bool acidImbue;
        public bool spiritBuff;
        public bool starBuff;
        public bool runeBuff;
        public bool poisonPotion;
        public bool soulPotion;
        public bool gremlinBuff;

        public int candyInBowl;
        private IList<string> candyFromTown = new List<string>();

        public override void UpdateBiomeVisuals()
        {
            bool showAurora = ((!player.ZoneDesert && player.ZoneSnow) || ZoneSpirit) && !Main.dayTime;
            bool reach = !Main.dayTime && ZoneReach;

            player.ManageSpecialBiomeVisuals("SpiritMod:AuroraSky", showAurora);
            player.ManageSpecialBiomeVisuals("SpiritMod:ReachSky", reach, player.Center);
            player.ManageSpecialBiomeVisuals("SpiritMod:BlueMoonSky", ZoneBlueMoon, player.Center);
            player.ManageSpecialBiomeVisuals("SpiritMod:SpiritSky", ZoneSpirit, player.Center);
            player.ManageSpecialBiomeVisuals("SpiritMod:WindEffect", windEffect, player.Center);
            player.ManageSpecialBiomeVisuals("SpiritMod:WindEffect2", windEffect2, player.Center);

            player.ManageSpecialBiomeVisuals("SpiritMod:Overseer", NPC.AnyNPCs(mod.NPCType("Overseer")));
            player.ManageSpecialBiomeVisuals("SpiritMod:IlluminantMaster", NPC.AnyNPCs(mod.NPCType("IlluminantMaster")));
            player.ManageSpecialBiomeVisuals("SpiritMod:Atlas", NPC.AnyNPCs(mod.NPCType("Atlas")));
        }

        public override void UpdateBiomes()
        {
            ZoneSpirit = MyWorld.SpiritTiles > 100;
            ZoneBlueMoon = MyWorld.BlueMoon;
            ZoneReach = MyWorld.ReachTiles > 100;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            MyPlayer modOther = other.GetSpiritPlayer();
            return ZoneSpirit == modOther.ZoneSpirit && ZoneReach == modOther.ZoneReach;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            MyPlayer modOther = other.GetSpiritPlayer();
            modOther.ZoneSpirit = ZoneSpirit;
            modOther.ZoneReach = ZoneReach;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            byte flags = 0;
            if (ZoneSpirit)
            {
                flags |= 1;
            }

            if (ZoneReach)
            {
                flags |= 2;
            }

            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            byte flags = reader.ReadByte();
            ZoneSpirit = ((flags & 1) == 1);
            ZoneReach = ((flags & 2) == 2);
        }


        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound
            {
                { "candyInBowl", candyInBowl },
                { "candyFromTown", candyFromTown }
            };
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            candyInBowl = tag.GetInt("candyInBowl");
            candyFromTown = tag.GetList<string>("candyFromTown");
        }


        public override void ResetEffects()
        {
            caltfist = false;
            firewall = false;
            hellCharm = false;
            bloodyBauble = false;
            amazonCharm = false;
            TormentLantern = false;
            phantomPet = false;
            QuacklingMinion = false;
            VampireCloak = false;
            SpiritCloak = false;
            HealCloak = false;
            astralSet = false;
            ChaosCrystal = false;
            twilightTalisman = false;
            ToxicExtract = false;
            gemPickaxe = false;
            cultistScarf = false;
            bismiteSet = false;
            scarabCharm = false;
            moonHeart = false;
            chitinSet = false;
            Fierysoul = false;
            infernalFlame = false;
            windEffect = false;
            windEffect2 = false;
            gremlinTooth = false;
            floranSet = false;
            atmos = false;
            SoulStone = false;
            anglure = false;
            geodeSet = false;
            manaWings = false;
            sunStone = false;
            fireMaw = false;
            moonStone = false;
            rogueSet = false;
            timScroll = false;
            wheezeScale = false;
            crystal = false;
            HellGaze = false;
            Bauble = false;
            geodeRanged = false;
            OverseerCharm = false;
            ReachSummon = false;
            hungryMinion = false;
            silkenSet = false;
            EaterSummon = false;
            CreeperSummon = false;
            CrystalShield = false;
            bloodfireShield = false;
            tankMinion = false;
            babyClamper = false;
            Phantom = false;
            IchorPendant = false;
            magnifyingGlass = false;
            magazine = false;
            daybloomSet = false;
            Ward = false;
            Ward1 = false;
            CursedPendant = false;
            BlueDust = false;
            SnakeMinion = false;
            Ghast = false;
            starCharm = false;
            eyezorEye = false;
            minionName = false;
            starMap = false;
            frigidGloves = false;
            NebulaPearl = false;
            TiteRing = false;
            bismiteShield = false;
            sacredVine = false;
            winterbornCharmMage = false;
            sepulchreCharm = false;
            clatterboneShield = false;
            KingRock = false;
            cthulhuMinion = false;
            leatherGlove = false;
            flametrail = false;
            icytrail = false;
            EnchantedPaladinsHammerMinion = false;
            ProbeMinion = false;
            crawlerockMinion = false;
            pigronMinion = false;
            skeletalonMinion = false;
            cragboundMinion = false;
            beetleMinion = false;
            shamanBand = false;
            briarHeart = false;
            lihzahrdMinion = false;
            aeonMinion = false;
            gasopodMinion = false;
            Flayer = false;
            steamMinion = false;
            DungeonSummon = false;
            OG = false;
            lavaRock = false;
            maskPet = false;
            starPet = false;
            bookPet = false;
            SwordPet = false;
            lanternPet = false;
            jellyfishPet = false;
            thrallPet = false;
            shadowPet = false;
            saucerPet = false;
            terror1Summon = false;
            terror2Summon = false;
            terror3Summon = false;
            terror4Summon = false;
            minior = false;
            drakomireMount = false;
            basiliskMount = false;
            toxify = false;
            gremlinBuff = false;
            spiritBuff = false;
            drakinMount = false;
            poisonPotion = false;
            starBuff = false;
            runeBuff = false;
            soulPotion = false;
            carnivorousPlantMinion = false;

            // Reset armor set booleans.
            duskSet = false;
            runicSet = false;
            primalSet = false;
            shadowSet = false;
            cometSet = false;
            meleeshadowSet = false;
            rangedshadowSet = false;
            magicshadowSet = false;
            witherSet = false;
            hellSet = false;
            quickSilverSet = false;
            reaperSet = false;
            spiritSet = false;
            coralSet = false;
            reaperMask = false;
            ichorSet1 = false;
            ichorSet2 = false;
            icySet = false;
            graniteSet = false;
            fierySet = false;
            putridSet = false;
            reachSet = false;
            duneSet = false;
            leatherSet = false;
            starSet = false;
            bloodfireSet = false;
            oceanSet = false;
            titanicSet = false;
            cryoChestplate = false;
            cryoSet = false;
            frigidSet = false;
            illuminantSet = false;
            windSet = false;
            marbleSet = false;
            crystalSet = false;
            magalaSet = false;
            depthSet = false;
            thermalSet = false;
            acidSet = false;
            infernalSet = false;
            bloomwindSet = false;
            veinstoneSet = false;
            clatterboneSet = false;
            lihzahrdSet = false;
            talonSet = false;

            marbleJustJumped = false;

            // Reset accessory booleans.
            OriRing = false;
            SRingOn = false;
            goldenApple = false;
            hpRegenRing = false;
            bubbleShield = false;
            animusLens = false;
            deathRose = false;
            mythrilCharm = false;
            spiritNecklace = false;
            KingSlayerFlask = false;
            DarkBough = false;
            Resolve = false;
            MoonSongBlossom = false;
            HolyGrail = false;
            infernalShield = false;
            shadowGauntlet = false;
            moonGauntlet = false;
            unboundSoulMinion = false;

            if (player.FindBuffIndex(mod.BuffType("BeetleFortitude")) < 0)
            {
                beetleStacks = 1;
            }

            if (player.FindBuffIndex(mod.BuffType("CollapsingVoid")) < 0)
            {
                voidStacks = 1;
            }

            phaseShift = false;
            blazeBurn = false;

            if (glyph != GlyphType.Phase)
            {
                phaseStacks = 0;
                phaseCounter = 0;
            }

            if (glyph != GlyphType.Veil)
            {
                veilCounter = 0;
            }

            if (glyph != GlyphType.Radiant)
            {
                divineStacks = 1;
                divineCounter = 0;
            }

            if (glyph != GlyphType.Storm)
            {
                stormStacks = 0;
            }

            if (frostCooldown > 0)
            {
                frostCooldown--;
            }

            frostRotation += Items.Glyphs.FrostGlyph.TURNRATE;
            if (frostRotation > MathHelper.TwoPi)
            {
                frostRotation -= MathHelper.TwoPi;
            }

            if (frostUpdate)
            {
                frostUpdate = false;
                if (glyph == GlyphType.Frost)
                {
                    Items.Glyphs.FrostGlyph.UpdateIceSpikes(player);
                }
            }

            frostCount = frostTally;
            frostTally = 0;

            copterFireFrame++;

            onGround = false;
            flying = false;
            swimming = false;
            if (player.velocity.Y != 0f)
            {
                if (player.mount.Active && player.mount.FlyTime > 0 && player.jump == 0 && player.controlJump && !player.mount.CanHover)
                {
                    flying = true;
                }
                else if (player.wet)
                {
                    swimming = true;
                }
            }
            else
            {
                onGround = true;
            }

            moving = false;
            if (player.velocity.X != 0f)
            {
                moving = true;
            }
        }
        public bool flag8 = false;
        public bool marbleJumpEffects = false;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (marbleSet && player.controlUp && player.releaseUp && marbleJump <= 0)
            {
                player.AddBuff(mod.BuffType("MarbleDivineWinds"), 120);
                Main.PlaySound(2, player.position, 20);
                for (int i = 0; i < 8; i++)
                {
                    int num = Dust.NewDust(player.position, player.width, player.height, 222, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if (Main.dust[num].position != player.Center)
                        Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                }
                marbleJump = 420;
            }
            if (marbleSet)
            {
                if (player.sliding || player.velocity.Y == 0f)
                {
                    player.justJumped = true;
                }
            }
            if (player.controlJump)
            {
                if (marbleJustJumped)
                {
                    marbleJustJumped = false;
                    flag8 = true;
                    if (flag8 && player.HasBuff(mod.BuffType("MarbleDivineWinds")))
                    {
                        if (Main.rand.Next(20) == 0)
                        {
                            Main.PlaySound(2, player.position, 24);
                        }
                        marbleJumpEffects = true;
                        int num23 = player.height;
                        if (player.gravDir == -1f)
                        {
                            num23 = 0;
                        }
                        player.velocity.Y = (0f - Player.jumpSpeed) * player.gravDir;
                        player.jump = (int)((double)Player.jumpHeight * 1.25);
                        for (int m = 0; m < 4; m++)
                        {
                            int num22 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)num23), player.width, 6, 222, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, 100, Color.White, .8f);
                            if (m % 2 == 0)
                            {
                                Dust expr_79B_cp_0 = Main.dust[num22];
                                expr_79B_cp_0.velocity.X = expr_79B_cp_0.velocity.X + (float)Main.rand.Next(10, 31) * 0.1f;
                            }
                            else
                            {
                                Dust expr_7CB_cp_0 = Main.dust[num22];
                                expr_7CB_cp_0.velocity.X = expr_7CB_cp_0.velocity.X - (float)Main.rand.Next(31, 71) * 0.1f;
                            }
                            Dust expr_7F9_cp_0 = Main.dust[num22];
                            expr_7F9_cp_0.velocity.Y = expr_7F9_cp_0.velocity.Y + (float)Main.rand.Next(-10, 31) * 0.1f;
                            Main.dust[num22].noGravity = true;
                            Main.dust[num22].scale += (float)Main.rand.Next(-10, 11) * .0025f;
                            Dust obj = Main.dust[num22];
                            obj.velocity *= Main.dust[num22].scale * 0.7f;
                            Vector2 value3 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                            value3.Normalize();
                            value3 *= (float)Main.rand.Next(81) * 0.08f;
                        }
                    }
                }
            }
            if (fierySet && player.controlDown && player.releaseDown && fierySetTimer <= 0)
            {
                Main.PlaySound(2, player.position, 74);
                for (int i = 0; i < 8; i++)
                {
                    int num = Dust.NewDust(player.position, player.width, player.height, 6, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if (Main.dust[num].position != player.Center)
                        Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                }
                for (int projFinder = 0; projFinder < 300; ++projFinder)
                {
                    if (Main.projectile[projFinder].sentry == true)
                    {
                        int p = Projectile.NewProjectile(Main.projectile[projFinder].Center.X, Main.projectile[projFinder].Center.Y - 20, 0f, 0f, mod.ProjectileType("FierySetExplosion"), Main.projectile[projFinder].damage, Main.projectile[projFinder].knockBack, player.whoAmI);                         
                    }
                    fierySetTimer = 480;

                }
            }
            if (SpiritMod.SpecialKey.JustPressed)
            {
                if (reaperMask && player.FindBuffIndex(mod.BuffType("WraithCooldown")) < 0)
                {
                    player.AddBuff(mod.BuffType("WraithCooldown"), 900);
                    player.AddBuff(mod.BuffType("Wraith"), 300);
                }

                if (daybloomSet && dazzleStacks >= 3600)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("Dazzle"), 0, 0, player.whoAmI);
                    Main.PlaySound(2, player.position, 9);
                    dazzleStacks = 0;
                }

                if (quickSilverSet && player.FindBuffIndex(mod.BuffType("SilverCooldown")) < 0)
                {
                    player.AddBuff(mod.BuffType("SilverCooldown"), 1800);

                    for (int h = 0; h < 12; h++)
                    {
                        Vector2 vel = new Vector2(0, -1);
                        float rand = Main.rand.NextFloat() * (float)(Math.PI * 2);

                        vel = vel.RotatedBy(rand);
                        vel *= 7f;

                        Vector2 mouse = Main.MouseScreen + Main.screenPosition;
                        Projectile.NewProjectile(mouse, vel, mod.ProjectileType("QuicksilverDroplet"), 62, 2, player.whoAmI);
                    }
                }

                if (cometSet && player.FindBuffIndex(mod.BuffType("StarCooldown")) < 0)
                {
                    player.AddBuff(mod.BuffType("StarCooldown"), 1800);

                    string[] projectileNames = new string[] { "Star1", "Star2", "Star3", "Star4", "Star5", "Star6" };
                    foreach (string name in projectileNames)
                    {
                        Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType(name), 75, 0, player.whoAmI);
                    }
                }

                if (reaperSet && player.FindBuffIndex(mod.BuffType("FelCooldown")) < 0)
                {
                    player.AddBuff(mod.BuffType("FelCooldown"), 2700);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("FelProj"), 0, 0, player.whoAmI);
                }

                if (depthSet && player.FindBuffIndex(mod.BuffType("SharkAttackBuff")) < 0)
                {
                    MyPlayer myPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    Rectangle textPos = new Rectangle((int)myPlayer.player.position.X, (int)myPlayer.player.position.Y - 60, myPlayer.player.width, myPlayer.player.height);

                    CombatText.NewText(textPos, new Color(29, 240, 255, 100), "Shark Attack!");
                    player.AddBuff(mod.BuffType("SharkAttackBuff"), 1800);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("SharkBlast"), 35, 0, player.whoAmI);
                }

                if (ichorSet1 && player.FindBuffIndex(mod.BuffType("GoreCooldown1")) < 0)
                {
                    player.AddBuff(mod.BuffType("GoreCooldown1"), 3600);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("Gores"), 4, 0, player.whoAmI);
                }

                if (ichorSet2 && player.FindBuffIndex(mod.BuffType("GoreCooldown2")) < 0)
                {
                    player.AddBuff(mod.BuffType("GoreCooldown2"), 3600);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("Gore1"), 21, 0, player.whoAmI);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("Gore1"), 21, 0, player.whoAmI);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("Gore1"), 21, 0, player.whoAmI);
                }
            }

            if (SpiritMod.HolyKey.JustPressed)
            {
                if (HolyGrail && player.FindBuffIndex(mod.BuffType("HolyCooldown")) < 0)
                {
                    player.AddBuff(mod.BuffType("HolyCooldown"), 3600);
                    player.AddBuff(mod.BuffType("HolyBuff"), 780);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("GrailWard"), 0, 0, player.whoAmI);
                }
            }

            if (SpiritMod.ReachKey.JustPressed)
            {
                if (deathRose && player.FindBuffIndex(mod.BuffType("DeathRoseCooldown")) < 0)
                {
                    player.AddBuff(mod.BuffType("DeathRoseCooldown"), 3600);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("PlantProj"), 0, 0, player.whoAmI);
                }
            }

        }

        public override bool PreItemCheck()
        {
            PrepareShotDetection();
            return true;
        }

        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
        {
            BeginShotDetection(item);
        }

        public override void PostItemCheck()
        {
            EndShotDetection();
        }

        private void PrepareShotDetection()
        {
            if (player.whoAmI == Main.myPlayer && !player.HeldItem.IsAir && !Main.gamePaused)
            {
                swingingItem = player.HeldItem;
            }
        }

        private void BeginShotDetection(Item item)
        {
            if (swingingItem == item)
            {
                swingingCheck = true;
            }
        }

        private void EndShotDetection()
        {
            swingingItem = null;
            swingingCheck = false;
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
           /* Item item = new Item();
            item.SetDefaults(mod.ItemType("OddKeystone"));
            items.Add(item);*/
        }

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (junk)
            {
                return;
            }

            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            if (player.ZoneDungeon && power >= 30 && Main.rand.NextBool(25))
            {
                caughtType = mod.ItemType("MysticalCage");
            }

            if (modPlayer.ZoneSpirit && NPC.downedMechBossAny && Main.rand.NextBool(player.cratePotion ? 35 : 65))
            {
                caughtType = mod.ItemType("SpiritCrate");
            }

            if (modPlayer.ZoneSpirit && NPC.downedMechBossAny && Main.rand.NextBool(5))
            {
                caughtType = mod.ItemType("SpiritKoi");
            }
            if (modPlayer.ZoneReach&& Main.rand.NextBool(5))
            {
                caughtType = mod.ItemType("ReachFishingCatch");
            }
            if (modPlayer.ZoneReach && Main.rand.NextBool(player.cratePotion ? 25 : 45))
            {
                caughtType = mod.ItemType("ReachCrate");
            }
            if (modPlayer.ZoneReach && Main.rand.NextBool(25))
            {
                caughtType = mod.ItemType("ThornDevilfish");
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (TiteRing && LastEnemyHit == victim && Main.rand.NextBool(10))
            {
                player.AddBuff(BuffID.ShadowDodge, 145);
            }

            if (hpRegenRing && LastEnemyHit == victim && Main.rand.NextBool(3))
            {
                player.AddBuff(BuffID.RapidHealing, 120);
            }

            if (OriRing && LastEnemyHit == victim && Main.rand.NextBool(10))
            {
                if (player.position.Y <= victim.position.Y)
                {
                    float distanceX = player.position.X - victim.position.X;  // change myplayer to nearest player in full version
                    float distanceY = player.position.Y - victim.position.Y; // change myplayer to nearest player in full version
                    float angle = (float)Math.Atan(distanceX / distanceY);

                    distXT = (float)(Math.Sin(angle) * 300);
                    distYT = (float)(Math.Cos(angle) * 300);

                    distX = player.position.X - distXT;
                    distY = player.position.Y - distYT;
                }

                if (player.position.Y > victim.position.Y)
                {
                    float distanceX = player.position.X - victim.position.X;  // change myplayer to nearest player in full version
                    float distanceY = player.position.Y - victim.position.Y; // change myplayer to nearest player in full version
                    float angle = (float)Math.Atan(distanceX / distanceY);

                    distXT = (float)(Math.Sin(angle) * 300);
                    distYT = (float)(Math.Cos(angle) * 300);

                    distX = (player.position.X + distXT);
                    distY = (player.position.Y + distYT);
                }

                Vector2 direction = victim.Center - player.Center;
                direction.Normalize();
                direction.X *= 20f;
                direction.Y *= 20f;

                float A = Main.rand.Next(-100, 100) * 0.01f;
                float B = Main.rand.Next(-100, 100) * 0.01f;

                Projectile.NewProjectile(distX, distY, direction.X + A, direction.Y + B, mod.ProjectileType("OriPetal"), 30, 1, player.whoAmI, 0f, 0f);
            }

            LastEnemyHit = victim;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {

            if (bloodyBauble)
            {
                if (Main.rand.Next(25) <= 1 && player.statLife != player.statLifeMax2)
                {
                    int lifeToHeal = 0;

                    if (player.statLife + damage / 4 <= player.statLifeMax2)
                        lifeToHeal = damage / 4;
                    else
                        lifeToHeal = player.statLifeMax2 - player.statLife;

                    player.statLife += lifeToHeal;
                    player.HealEffect(lifeToHeal);
                }
            }
            if (frigidGloves && crit)
            {
                if (Main.rand.NextBool(2))
                {
                    player.AddBuff(BuffID.Frostburn, 180);
                }
            }

            if (virulence <= 0f)
            {
                Projectile.NewProjectile(target.position, Vector2.Zero, mod.ProjectileType("VirulenceExplosion"), 55, 8, Main.myPlayer);
                virulence = 600f;
                player.AddBuff(mod.BuffType("VirulenceCooldown"), 140);
            }

            if (duneSet && item.thrown)
            {
                GNPC info = target.GetGlobalNPC<GNPC>();
                if (info.duneSetStacks++ >= 4)
                {
                    player.AddBuff(mod.BuffType("DesertWinds"), 180);
                    Projectile.NewProjectile(player.position.X + 20, player.position.Y, 0, -2, mod.ProjectileType("DuneKnife"), 40, 0, Main.myPlayer);
                    info.duneSetStacks = 0;
                }
            }

            if (icySet && item.magic && Main.rand.NextBool(14))
            {
                player.AddBuff(mod.BuffType("BlizzardWrath"), 240);
            }

            if (meleeshadowSet && Main.rand.NextBool(10) && item.melee)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (magicshadowSet && Main.rand.NextBool(10) && item.magic)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (magicshadowSet && Main.rand.NextBool(10) && item.summon)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (rangedshadowSet && Main.rand.NextBool(10) && item.ranged)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, -12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (rangedshadowSet && Main.rand.NextBool(10) && item.thrown)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, -12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (spiritNecklace && Main.rand.NextBool(10) && item.melee)
            {
                target.AddBuff(mod.BuffType("EssenceTrap"), 240);
                damage += target.defense;
            }

            if (reaperSet && Main.rand.NextBool(15))
            {
                target.AddBuff(mod.BuffType("FelBrand"), 160);
            }

            if (magalaSet && Main.rand.NextBool(6))
            {
                target.AddBuff(mod.BuffType("FrenzyVirus"), 240);
            }

            if (wheezeScale && Main.rand.NextBool(9) && item.melee)
            {
                Vector2 vel = new Vector2(0, -1);
                float rand = Main.rand.NextFloat() * 6.283f;
                vel = vel.RotatedBy(rand);
                vel *= 8f;
                Projectile.NewProjectile(target.Center, vel, mod.ProjectileType("Wheeze"), item.damage / 2, 0, Main.myPlayer);
            }

            if (ToxicExtract && Main.rand.NextBool(5) && item.magic)
            {
                target.AddBuff(BuffID.Venom, 240);
            }

            if (magalaSet && (item.magic || item.ranged || item.melee || item.thrown) && Main.rand.NextBool(14))
            {
                player.AddBuff(mod.BuffType("FrenzyVirus1"), 240);
            }

            if (sunStone && item.melee && Main.rand.NextBool(18))
            {
                target.AddBuff(mod.BuffType("SunBurn"), 240);
            }

            if (geodeSet && crit && Main.rand.NextBool(5))
            {
                target.AddBuff(mod.BuffType("Crystal"), 180);
            }

            if (gremlinBuff && item.melee)
            {
                target.AddBuff(BuffID.Poisoned, 120);
            }
            if (amazonCharm && item.melee && Main.rand.Next(10) == 0)
            {
                target.AddBuff(BuffID.Poisoned, 120);
            }
            if (hellCharm && item.melee && Main.rand.Next(10) == 0)
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
            if (infernalFlame && item.melee)
            {
                if (crit)
                {
                    if (Main.rand.NextBool(12))
                    {
                        Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("PhoenixProjectile"), 50, 4, Main.myPlayer);
                    }
                }
            }
        }

        int Charger;
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
             if (bloodyBauble)
            {
                if (Main.rand.Next(25) <= 1 && player.statLife != player.statLifeMax2)
                {
                    int lifeToHeal = 0;

                    if (player.statLife + damage / 4 <= player.statLifeMax2)
                        lifeToHeal = damage / 4;
                    else
                        lifeToHeal = player.statLifeMax2 - player.statLife;

                    player.statLife += lifeToHeal;
                    player.HealEffect(lifeToHeal);
                }
            }
            if (sacredVine)
            {
                if (Main.rand.NextBool(11))
                {
                    player.AddBuff(BuffID.Regeneration, 180);
                }
            }

            if (virulence <= 0f)
            {
                Projectile.NewProjectile(target.position, Vector2.Zero, mod.ProjectileType("VirulenceExplosion"), 55, 8, Main.myPlayer);
                virulence = 600f;
                player.AddBuff(mod.BuffType("VirulenceCooldown"), 140);
            }

            if (reaperSet && Main.rand.NextBool(15))
            {
                target.AddBuff(mod.BuffType("FelBrand"), 160);
            }

            if (KingRock && Main.rand.NextBool(5) && proj.magic)
            {
                Projectile.NewProjectile(player.position.X + Main.rand.Next(-350, 350), player.position.Y - 350, 0, 12, mod.ProjectileType("PrismaticBolt"), 55, 0, Main.myPlayer);
                Projectile.NewProjectile(player.position.X + Main.rand.Next(-350, 350), player.position.Y - 350, 0, 12, mod.ProjectileType("PrismaticBolt"), 55, 0, Main.myPlayer);
            }

            if (magalaSet && proj.thrown)
            {
                target.AddBuff(mod.BuffType("FrenzyVirus"), 180);
            }

            if (geodeSet && crit && Main.rand.NextBool(5))
            {
                target.AddBuff(mod.BuffType("Crystal"), 180);
            }
            if (amazonCharm && Main.rand.Next(12) == 0)
            {
                target.AddBuff(BuffID.Poisoned, 120);
            }
            if (hellCharm && Main.rand.Next(12) == 0)
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
            if (geodeRanged && proj.ranged && Main.rand.NextBool(24))
            {
                target.AddBuff(BuffID.Frostburn, 180);
                target.AddBuff(BuffID.OnFire, 180);
                target.AddBuff(BuffID.CursedInferno, 180);
            }

            if (shamanBand && proj.magic && Main.rand.NextBool(9))
            {
                target.AddBuff(BuffID.OnFire, 180);
            }

            if (briarHeart && proj.magic)
            {
                if (Main.rand.NextBool(9))
                {
                    target.AddBuff(BuffID.CursedInferno, 180);
                    target.AddBuff(BuffID.Ichor, 180);
                }

                if (Main.rand.NextBool(3))
                {
                    player.AddBuff(mod.BuffType("ToothBuff"), 300);
                }
            }

            if (bloodfireSet && proj.magic)
            {
                if (Main.rand.NextBool(15))
                {
                    target.AddBuff(mod.BuffType("BCorrupt"), 180);
                }

                if (Main.rand.NextBool(30))
                {
                    player.statLife += 2;
                    player.HealEffect(2);
                }
            }

            if (eyezorEye && proj.magic && crit && Main.rand.NextBool(3))
            {
                target.StrikeNPC(40, 0f, 0, crit);
            }

            if (sunStone && proj.melee && Main.rand.NextBool(18))
            {
                target.AddBuff(mod.BuffType("SunBurn"), 240);
            }

            if (moonStone && proj.ranged && Main.rand.NextBool(18))
            {
                target.AddBuff(mod.BuffType("MoonBurn"), 240);
            }

            if (wheezeScale && Main.rand.NextBool(9) && proj.melee)
            {
                Vector2 vel = new Vector2(0, -1);
                float rand = Main.rand.NextFloat() * 6.283f;
                vel = vel.RotatedBy(rand);
                vel *= 8f;
                Projectile.NewProjectile(target.Center, vel, mod.ProjectileType("Wheeze"), Main.hardMode ? 40 : 20, 0, player.whoAmI);
            }

            if (DarkBough && proj.minion)
            {
                if (Main.rand.NextBool(15))
                {
                    for (int h = 0; h < 6; h++)
                    {
                        Vector2 vel = new Vector2(0, -1);
                        float rand = Main.rand.NextFloat() * 6.283f;
                        vel = vel.RotatedBy(rand);
                        vel *= 8f;
                        Projectile.NewProjectile(target.Center, vel, mod.ProjectileType("NightmareBarb"), 29, 1, player.whoAmI);
                    }
                }

                if (Main.rand.NextBool(30))
                {
                    player.statLife += 2;
                    player.HealEffect(2);
                }
            }

            if (magazine && proj.ranged && ++Charger > 10)
            {
                crit = true;
                Charger = 0;
            }

            if (windSet && proj.minion && Main.rand.NextBool(6))
            {
                for (int i = 0; i <= 3; i++)
                {
                    Projectile.NewProjectile(target.Center.X, target.Center.Y, Main.rand.Next(-2, 4), -5, mod.ProjectileType("DeitySoul2"), 39, 1, player.whoAmI);
                }
            }

            if (magalaSet && (proj.melee || proj.minion || proj.magic || proj.ranged))
            {
                target.AddBuff(mod.BuffType("FrenzyVirus"), 180);
            }

            if (acidImbue && Main.rand.NextBool(11) && proj.thrown)
            {
                target.AddBuff(mod.BuffType("AcidBurn"), 240);
            }

            if (spiritNecklace && proj.thrown)
            {
                target.AddBuff(mod.BuffType("EssenceTrap"), 180);
            }

            if (timScroll && proj.magic)
            {
                switch (Main.rand.Next(12))
                {
                    case 0:
                        target.AddBuff(BuffID.OnFire, 120);
                        break;

                    case 1:
                        target.AddBuff(BuffID.Venom, 120);
                        break;

                    case 2:
                        target.AddBuff(BuffID.CursedInferno, 120);
                        break;

                    case 3:
                        target.AddBuff(BuffID.Frostburn, 120);
                        break;

                    case 4:
                        target.AddBuff(BuffID.Confused, 120);
                        break;

                    case 5:
                        target.AddBuff(BuffID.ShadowFlame, 120);
                        break;

                    default:
                        break;
                }
            }

            if (gremlinBuff)
            {
                target.AddBuff(BuffID.Poisoned, 120);
            }

            if (Fierysoul && proj.minion && Main.rand.NextBool(14))
            {
                target.AddBuff(BuffID.OnFire, 240);
            }

            if (crystalSet && proj.minion && Main.rand.NextBool(15))
            {
                target.AddBuff(mod.BuffType("SoulBurn"), 240);
            }

            if (KingSlayerFlask && proj.thrown && Main.rand.NextBool(10))
            {
                target.AddBuff(mod.BuffType("KingslayerVenom"), 300);
            }

            if (spiritNecklace && proj.melee && Main.rand.NextBool(10))
            {
                target.AddBuff(mod.BuffType("EssenceTrap"), 180);
                damage += target.defense >> 1;
            }

            if (winterbornCharmMage && proj.magic && Main.rand.NextBool(7))
            {
                target.AddBuff(mod.BuffType("MageFreeze"), 180);
            }
            if (cryoChestplate && proj.thrown && Main.rand.NextBool(8))
            {
                target.AddBuff(mod.BuffType("MageFreeze"), 180);
            }
            if (putridSet && proj.ranged && ++Rangedhits >= 4)
            {
                Projectile.NewProjectile(proj.position, Vector2.Zero, mod.ProjectileType("CursedFlame"), proj.damage, 0f, proj.owner);
                Rangedhits = 0;
            }

            if (spiritNecklace && (proj.minion || proj.magic || proj.ranged) && Main.rand.NextBool(10))
            {
                target.AddBuff(mod.BuffType("EssenceTrap"), 180);
            }

            if (magicshadowSet && Main.rand.NextBool(4) && proj.magic)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (meleeshadowSet && Main.rand.NextBool(4) && proj.melee)
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (rangedshadowSet && Main.rand.Next(4) == 2 && (proj.ranged || proj.thrown))
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, mod.ProjectileType("SpiritShardFriendly"), 60, 0, Main.myPlayer);
            }

            if (ToxicExtract && Main.rand.NextBool(5) && proj.magic)
            {
                target.AddBuff(BuffID.Venom, 240);
            }

            if (infernalFlame && proj.melee && crit && Main.rand.NextBool(8))
            {
                Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("PhoenixProjectile"), 50, 4, player.whoAmI);
            }

            if (NebulaPearl && Main.rand.NextBool(8) && proj.magic)
            {
                Item.NewItem(target.Hitbox, 3454);
            }

            if (hellSet && Main.rand.NextBool(8) && proj.ranged)
            {
                for (int h = 0; h < 4; h++)
                {
                    Vector2 vel = new Vector2(0, -1);
                    float rand = Main.rand.NextFloat() * 6.283f;
                    vel = vel.RotatedBy(rand);
                    vel *= 4f;
                    Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, 15, 40, 0, Main.myPlayer);
                }

                Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 0, mod.ProjectileType("FireExplosion"), 39, 0, Main.myPlayer);
            }

            if (duneSet && proj.thrown)
            {
                GNPC info = target.GetGlobalNPC<GNPC>();
                if (info.duneSetStacks++ >= 4)
                {
                    player.AddBuff(mod.BuffType("DesertWinds"), 180);
                    Projectile.NewProjectile(player.position.X + 20, player.position.Y, 0, -2, mod.ProjectileType("DuneKnife"), 40, 0, Main.myPlayer);
                    info.duneSetStacks = 0;
                }
            }

            if (icySet && proj.magic && Main.rand.NextBool(14))
            {
                player.AddBuff(mod.BuffType("BlizzardWrath"), 240);
            }

            if (lihzahrdSet && proj.thrown && Main.rand.NextBool(10))
            {
                Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("LihzahrdExplosion"), 50, 4, player.whoAmI);
                Projectile.NewProjectile(target.Center, new Vector2(0.2f, 0f), mod.ProjectileType("BabyLihzahrd"), 41, 4, player.whoAmI);
                Projectile.NewProjectile(target.Center, new Vector2(0.1f, 0f), mod.ProjectileType("BabyLihzahrd"), 41, 4, player.whoAmI);
            }

            if (magalaSet && Main.rand.NextBool(6))
            {
                player.AddBuff(mod.BuffType("FrenzyVirus1"), 240);
            }

            if (titanicSet && proj.melee)
            {
                GNPC info = target.GetGlobalNPC<GNPC>();
                if (info.titanicSetStacks++ >= 4)
                {
                    Projectile.NewProjectile(player.position.X + 20, player.position.Y + 30, 0, 12, mod.ProjectileType("WaterMass"), 60, 0, Main.myPlayer);
                    info.titanicSetStacks = 0;
                }
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Main.rand.NextBool(6) && sepulchreCharm)
            {
                for (int k = 0; k < 5; k++)
                {
                    int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, 75, 0f, 0f, 100, default, .84f);
                }

                player.longInvince = true;
            }
            else
            {
                player.longInvince = false;
            }

            if (ActiveDash == DashType.Shinigami)
            {
                return false;
            }

            int index = player.FindBuffIndex(mod.BuffType("PhantomVeil"));
            if (index >= 0)
            {
                player.DelBuff(index);
                Items.Glyphs.VeilGlyph.Block(player);
                veilCounter = 0;

                return false;
            }

            if (bubbleTimer > 0)
            {
                return false;
            }

            return true;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            veilCounter = 0;

            if (glyph == GlyphType.Daze && Main.rand.NextBool(2))
            {
                player.AddBuff(BuffID.Confused, 180);
            }

            if (rogueSet && !player.HasBuff(mod.BuffType("RogueCooldown")))
            {
                player.AddBuff(BuffID.Invisibility, 200);
                player.AddBuff(mod.BuffType("RogueCooldown"), 1520);
            }

            if (leatherSet)
            {
                concentratedCooldown = 360;
                concentrated = false;
            }
            if (cryoSet)
            {
                cryoTimer = 0;
            }
            if (bismiteSet)
            {
                virulence = 600f;

                if (!player.HasBuff(mod.BuffType("VirulenceCooldown")))
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("VirulenceExplosion"), 15, 5, Main.myPlayer);
                }

                player.AddBuff(mod.BuffType("VirulenceCooldown"), 140);
            }

            if (SRingOn)
            {
                for (int h = 0; h < 3; h++)
                {
                    Vector2 vel = new Vector2(0, -1);
                    float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
                    vel = vel.RotatedBy(rand);
                    vel *= 2f;
                    Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, 297, 45, 0, Main.myPlayer);
                }
            }

            if (moonHeart)
            {
                int n = 5;
                for (int i = 0; i < n; i++)
                {
                    Vector2 vel = new Vector2(0, -1);
                    float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
                    vel = vel.RotatedBy(rand);
                    vel *= 5f;
                    Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, mod.ProjectileType("AlienSpit"), 65, 0, Main.myPlayer);
                }
            }

            if (cryoSet)
            {
                Main.PlaySound(2, player.position, 50);
            }

            if (ChaosCrystal && Main.rand.Next(4) == 1)
            {
                bool canSpawn = false;
                int teleportStartX = (int)(Main.player[Main.myPlayer].position.X / 16) - 35;
                int teleportRangeX = 70;
                int teleportStartY = (int)(Main.player[Main.myPlayer].position.Y / 16) - 35;
                int teleportRangeY = 70;
                Vector2 vector2 = TestTeleport(ref canSpawn, teleportStartX, teleportRangeX, teleportStartY, teleportRangeY);

                if (canSpawn)
                {
                    Vector2 newPos = vector2;
                    Main.player[Main.myPlayer].Teleport(newPos, 2, 0);
                    Main.player[Main.myPlayer].velocity = Vector2.Zero;

                    if (Main.netMode == 2)
                    {
                        RemoteClient.CheckSection(Main.myPlayer, Main.player[Main.myPlayer].position, 1);
                        NetMessage.SendData(65, -1, -1, null, 0, Main.myPlayer, newPos.X, newPos.Y, 3, 0, 0);
                    }
                }

            }

            // IRIAZUL
            if (veinstoneSet && Main.rand.NextBool(8))
            {
                int amount = Main.rand.Next(2, 5);
                for (int i = 0; i < amount; ++i)
                {
                    Vector2 position = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201), player.Center.Y - 600f);
                    position.X = (position.X * 10f + player.position.X) / 11f + Main.rand.Next(-100, 101);
                    position.Y -= 150;

                    float speedX = player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
                    float speedY = player.Center.Y - position.Y;
                    if (speedY < 0f)
                    {
                        speedY *= -1f;
                    }

                    if (speedY < 20f)
                    {
                        speedY = 20f;
                    }

                    float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    length = 12 / length;

                    speedX *= length;
                    speedY *= length;
                    speedX += Main.rand.Next(-40, 41) * 0.03f;
                    speedY += Main.rand.Next(-40, 41) * 0.03f;
                    speedX *= Main.rand.Next(75, 150) * 0.01f;

                    position.X += Main.rand.Next(-50, 51);
                    Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType("VeinstoneBlood"), 40, 1, player.whoAmI);
                }
            }

            if (acidSet && Main.rand.NextBool(3))
            {
                Projectile.NewProjectile(player.position, new Vector2(0, -2), mod.ProjectileType("AcidBlast"), 25, 0, Main.myPlayer);
            }

            if (infernalSet && Main.rand.NextBool(10))
            {
                Projectile.NewProjectile(player.position, new Vector2(0, -2), mod.ProjectileType("InfernalBlast"), 50, 7, Main.myPlayer);
            }

            if (starCharm)
            {
                int amount = Main.rand.Next(4, 6);
                for (int i = 0; i < amount; ++i)
                {
                    Vector2 position = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201), player.Center.Y - 600f);
                    position.X = (position.X * 10f + player.position.X) / 11f + Main.rand.Next(-100, 101);
                    position.Y -= 150;

                    float speedX = player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
                    float speedY = player.Center.Y - position.Y;
                    if (speedY < 0f)
                    {
                        speedY *= -1f;
                    }

                    if (speedY < 20f)
                    {
                        speedY = 20f;
                    }

                    float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    length = 12 / length;

                    speedX *= length;
                    speedY *= length;
                    speedX = speedX + Main.rand.Next(-40, 41) * 0.03f;
                    speedY = speedY + Main.rand.Next(-40, 41) * 0.03f;
                    speedX *= Main.rand.Next(75, 150) * 0.01f;

                    position.X += Main.rand.Next(-50, 51);
                    Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType("Starshock1"), 35, 1, player.whoAmI);
                }
            }

            if (gremlinTooth && Main.rand.NextBool(3))
            {
                player.AddBuff(mod.BuffType("ToothBuff"), 300);
            }

            if (starMap && Main.rand.NextBool(3))
            {
                int amount = Main.rand.Next(2, 3);
                for (int i = 0; i < amount; ++i)
                {
                    Vector2 position = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(-300, 301), player.Center.Y - 800f);
                    position.X = (position.X * 10f + player.position.X) / 11f + Main.rand.Next(-100, 101);
                    position.Y -= 150;

                    float speedX = player.position.X + player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
                    float speedY = player.Center.Y - position.Y;
                    if (speedY < 0f)
                    {
                        speedY *= -1f;
                    }

                    if (speedY < 30f)
                    {
                        speedY = 30f;
                    }

                    float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    length = 12 / length;

                    speedX *= length;
                    speedY *= length;
                    speedX = speedX + Main.rand.Next(-40, 41) * 0.03f;
                    speedY = speedY + Main.rand.Next(-40, 41) * 0.03f;
                    speedX *= Main.rand.Next(75, 150) * 0.01f;

                    position.X += Main.rand.Next(-10, 11);
                    Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType("Starshock1"), 24, 1, player.whoAmI);
                }
            }

            if (Bauble && player.statLife < (player.statLifeMax2 >> 1) && baubleTimer <= 0)
            {
                Projectile.NewProjectile(Main.player[Main.myPlayer].Center, Vector2.Zero, mod.ProjectileType("IceReflector"), 0, 0, Main.myPlayer);
                player.endurance += .30f;
                baubleTimer = 7200;
            }

            if (OverseerCharm)
            {
                for (int h = 0; h < 8; h++)
                {
                    Vector2 vel = new Vector2(0, -1);
                    float rand = Main.rand.NextFloat() * 6.283f;
                    vel = vel.RotatedBy(rand);
                    vel *= 4f;
                    int proj = Projectile.NewProjectile(Main.player[Main.myPlayer].Center, vel, mod.ProjectileType("SpiritShardFriendly"), 250, 0, Main.myPlayer);
                }
            }

            if (babyClamper == true && Main.rand.NextBool(6))
            {
                Projectile.NewProjectile(player.position.X + 20, player.position.Y, 0, -2, mod.ProjectileType("ClampOrb"), 0, 0, Main.myPlayer);
                player.endurance += 0.1F;
            }

            if (mythrilCharm && Main.rand.NextBool(2))
            {
                int mythrilCharmDamage = (int)(damage / 4);
                if (mythrilCharmDamage < 1)
                {
                    mythrilCharmDamage = 5;
                }

                Rectangle mythrilCharmCollision = new Rectangle((int)player.Center.X - 120, (int)player.Center.Y - 120, 240, 240);
                for (int i = 0; i < 200; ++i)
                {
                    if (Main.npc[i].active && Main.npc[i].Hitbox.Intersects(mythrilCharmCollision))
                    {
                        Main.npc[i].StrikeNPCNoInteraction(mythrilCharmDamage, 0, 0);
                    }
                }

                for (int i = 0; i < 15; ++i)
                {
                    Dust.NewDust(new Vector2(mythrilCharmCollision.X, mythrilCharmCollision.Y), mythrilCharmCollision.Width, mythrilCharmCollision.Height, DustID.LunarOre);
                }
            }
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (SRingOn)
            {
                int newProj = Projectile.NewProjectile(player.Center, new Vector2(6, 6), 356, 40, 0f, Main.myPlayer);

                int dist = 800;
                int target = -1;
                for (int i = 0; i < 200; ++i)
                {
                    if (Main.npc[i].active && Main.npc[i].CanBeChasedBy(Main.projectile[newProj], false))
                    {
                        if ((Main.npc[i].Center - Main.projectile[newProj].Center).Length() < dist)
                        {
                            target = i;
                            break;
                        }
                    }
                }

                Main.projectile[newProj].ai[0] = target;
            }

            if (cryoSet)
            {
                quiet = true;
            }

            if (Fierysoul)
            {
                Projectile.NewProjectile(player.Center, new Vector2(6, 6), ProjectileID.MolotovFire2, 30, 0f, Main.myPlayer);
            }

            if (soulPotion && Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("SoulPotionWard"), 0, 0f, Main.myPlayer);
            }

            if (spiritBuff && Main.rand.NextBool(3))
            {
                int newProj = Projectile.NewProjectile(player.Center, new Vector2(6, 6), mod.ProjectileType("StarSoul"), 40, 0f, Main.myPlayer);

                int dist = 800;
                int target = -1;
                for (int i = 0; i < 200; ++i)
                {
                    if (Main.npc[i].active && Main.npc[i].CanBeChasedBy(Main.projectile[newProj], false))
                    {
                        if ((Main.npc[i].Center - Main.projectile[newProj].Center).Length() < dist)
                        {
                            target = i;
                            break;
                        }
                    }
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (fateToken)
            {
                player.statLife = 500;
                timeLeft = 0;
                Main.NewText("Fate has protected you");
                fateToken = false;
                return false;
            }

            if (bubbleShield)
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    int type = player.armor[i].type;
                    if (type == mod.ItemType("BubbleShield"))
                    {
                        player.armor[i].SetDefaults(0);
                        break;
                    }
                }

                player.statLife = 150;
                bubbleTimer = 360;
                return false;
            }

            if (clatterboneSet && clatterboneTimer <= 0)
            {
                player.AddBuff(mod.BuffType("Sturdy"), 21600);
                MyPlayer myPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                Rectangle textPos = new Rectangle((int)myPlayer.player.position.X, (int)myPlayer.player.position.Y - 60, myPlayer.player.width, myPlayer.player.height);
                CombatText.NewText(textPos, new Color(29, 240, 255, 100), "Sturdy Activated!");

                player.statLife += (int)damage;
                clatterboneTimer = 21600; // 6 minute timer.

                return false;
            }

            if (damageSource.SourceOtherIndex == 8)
            {
                CustomDeath(ref damageSource);
            }

            return true;
        }

        private void CustomDeath(ref PlayerDeathReason reason)
        {
            if (player.FindBuffIndex(mod.BuffType("BurningRage")) >= 0)
            {
                reason = PlayerDeathReason.ByCustomReason(player.name + " was consumed by Rage.");
            }
        }


        public override void PreUpdate()
        {
            if (fierySet)
            {
                fierySetTimer--;
            }
            else
            {
                fierySetTimer = 480;
            }
            if (fierySetTimer == 0)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
                for (int i = 0; i < 2; i++)
                {
                    int num = Dust.NewDust(player.position, player.width, player.height, 6, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if (Main.dust[num].position != player.Center)
                        Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
            if (marbleSet)
            {
                marbleJump--;
                marbleJustJumped = true;
            }
            if (marbleJump == 0)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
                for (int i = 0; i < 2; i++)
                {
                    int num = Dust.NewDust(player.position, player.width, player.height, 222, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if (Main.dust[num].position != player.Center)
                        Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
            if (!marbleSet)
            {
                marbleJustJumped = false;
            }
            if ((player.velocity.Y == 0f || player.sliding || (player.autoJump && player.justJumped)) && marbleJustJumped)
            {
                marbleJustJumped = true;
            }
            if (graniteSet)
            {
                int num323;
                int num326;
                if (player.velocity.Y == 0f && player.HasBuff(mod.BuffType("GraniteBonus")))
                {
                    num326 = 1;
                    num326 += player.extraFall;
                    num323 = (int)((player.position.Y / 16f) - player.fallStart) / 2;
                    if (num323 >= 8)
                    {
                        num323 = 8;
                    }
                    {
                        if (player.gravDir == 1f && num323 > num326)
                        {
                            player.ClearBuff(mod.BuffType("GraniteBonus"));
                            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 109));
                            {
                                for (int i = 0; i < 8 * num323; i++)
                                {
                                    int num = Dust.NewDust(player.position, player.width, player.height, 226, 0f, -2f, 0, default(Color), 2f);
                                    Main.dust[num].noGravity = true;
                                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                                    Main.dust[num].scale *= .25f;
                                    if (Main.dust[num].position != player.Center)
                                        Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                                }
                            }
                            int proj = Projectile.NewProjectile(player.position.X, player.position.Y,
                                0, 0, mod.ProjectileType("GraniteSpike1"), num323 * 10, 0, player.whoAmI);
                            int proj1 = Projectile.NewProjectile(player.position.X, player.position.Y,
                               0, 0, mod.ProjectileType("StompExplosion"), num323 * 10, 6, player.whoAmI);
                            Main.projectile[proj].timeLeft = 0;
                            Main.projectile[proj].ranged = true;

                        }
                    }
                    stompCooldown = 600;
                }
                stompCooldown--;
                if (stompCooldown == 0)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(player.position, player.width, player.height, 226, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .25f;
                        if (Main.dust[num].position != player.Center)
                            Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                    }

                }
            }
            if (!Main.dayTime && MyWorld.dayTimeSwitched)
            {
                candyInBowl = 2;
                candyFromTown.Clear();
            }

            if (Main.rand.NextBool(12) && ZoneReach && player.ZoneOverworldHeight && !player.ZoneBeach && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneJungle && !player.ZoneHoly)
            {
                float goreScale = 0.01f * Main.rand.Next(20, 70);
                int a = Gore.NewGore(new Vector2(player.Center.X + Main.rand.Next(-1000, 1000), player.Center.Y + (Main.rand.Next(-1000, -100))), new Vector2(Main.windSpeed * 3f, 0f), 911, goreScale);
                Main.gore[a].timeLeft = 15;
                Main.gore[a].rotation = 0f;
                Main.gore[a].velocity = new Vector2(Main.windSpeed * 40f, Main.rand.NextFloat(0.2f, 2f));
            }

            if (windEffect)
            {
                if (Main.windSpeed <= -.01f)
                {
                    Main.windSpeed = -.8f; ;
                }

                if (Main.windSpeed >= .01f)
                {
                    Main.windSpeed = .8f;
                }
            }

            CalculateSpeed();

            if (player.whoAmI == Main.myPlayer)
            {
                if (!player.HeldItem.IsAir)
                {
                    glyph = player.HeldItem.GetGlobalItem<Items.GItem>().Glyph;
                    if (glyph == GlyphType.None && player.nonTorch >= 0 && player.nonTorch != player.selectedItem)
                    {
                        if (!player.inventory[player.nonTorch].IsAir)
                        {
                            glyph = player.inventory[player.nonTorch].GetGlobalItem<Items.GItem>().Glyph;
                        }
                    }
                }
                else
                {
                    glyph = GlyphType.None;
                }

                if (Main.netMode == 1)
                {
                    ModPacket packet = SpiritMod.instance.GetPacket(MessageType.PlayerGlyph, 2);
                    packet.Write((byte)Main.myPlayer);
                    packet.Write((byte)glyph);
                    packet.Send();
                }
            }

            if (glyph == GlyphType.Bee)
            {
                player.AddBuff(BuffID.Honey, 2);
            }
            else if (glyph == GlyphType.Phase)
            {
                if (phaseStacks < 3)
                {
                    phaseCounter++;
                    if (phaseCounter >= 12 * 60)
                    {
                        phaseCounter = 0;
                        phaseStacks++;
                        player.AddBuff(mod.BuffType("TemporalShift"), 2);
                    }
                }
            }
            else if (glyph == GlyphType.Veil)
            {
                veilCounter++;
                if (veilCounter >= 8 * 60)
                {
                    veilCounter = 0;
                    player.AddBuff(mod.BuffType("PhantomVeil"), 2);
                }
            }
            else if (glyph == GlyphType.Void)
            {
                Items.Glyphs.VoidGlyph.DevouringVoid(player);
            }
            else if (glyph == GlyphType.Radiant)
            {
                divineCounter++;
                if (divineCounter >= 60)
                {
                    divineCounter = 0;
                    player.AddBuff(mod.BuffType("DivineStrike"), 2);
                }
            }

            if (icytrail && player.velocity.X != 0)
            {
                Projectile.NewProjectile(player.position.X, player.position.Y + 40, 0f, 0f, mod.ProjectileType("FrostTrail"), 35, 0f, player.whoAmI);
            }

            if (starSet && player.velocity.X != 0 && Main.rand.NextBool(10))
            {
                Projectile.NewProjectile(player.position.X, player.position.Y + 40, 0f, 0f, mod.ProjectileType("StarTrail"), 13, 0f, player.whoAmI);
            }

            if (flametrail && player.velocity.X != 0)
            {
                Projectile.NewProjectile(player.position.X, player.position.Y + 40, 0f, 0f, mod.ProjectileType("CursedFlameTrail"), 35, 0f, player.whoAmI);
            }

            if (CrystalShield && player.velocity.X != 0 && Main.rand.NextBool(3))
            {
                if (player.velocity.X < 0)
                {
                    Projectile.NewProjectile(player.position.X, player.Center.Y, Main.rand.Next(6, 10), Main.rand.Next(-3, 3), 90, 36, 0f, player.whoAmI);
                }

                if (player.velocity.X > 0)
                {
                    Projectile.NewProjectile(player.position.X, player.Center.Y, Main.rand.Next(-10, -6), Main.rand.Next(-3, 3), 90, 36, 0f, player.whoAmI);
                }
            }
        }

        private void CalculateSpeed()
        {
            //Mimics the Stopwatch accessory
            float slice = player.velocity.Length();
            int count = (int)(1f + slice * 6f);
            if (count > phaseSlice.Length)
            {
                count = phaseSlice.Length;
            }

            for (int i = count - 1; i > 0; i--)
            {
                phaseSlice[i] = phaseSlice[i - 1];
            }

            phaseSlice[0] = slice;
            float inverse = 1f / count;
            float sum = 0f;
            for (int n = 0; n < phaseSlice.Length; n++)
            {
                if (n < count)
                {
                    sum += phaseSlice[n];
                }
                else
                {
                    phaseSlice[n] = sum * inverse;
                }
            }

            sum *= inverse;
            float boost = sum * (216000 / 42240f);
            if (!player.merman && !player.ignoreWater)
            {
                if (player.honeyWet)
                {
                    boost *= .25f;
                }
                else if (player.wet)
                {
                    boost *= .5f;
                }
            }

            SpeedMPH = (float)Math.Round(boost);
        }

        public override void UpdateBadLifeRegen()
        {
            int before = player.lifeRegen;
            bool drain = false;

            if (DoomDestiny)
            {
                drain = true;
                player.lifeRegen -= 16;
            }

            if (blazeBurn)
            {
                drain = true;
                player.lifeRegen -= 10;
            }

            if (drain && before > 0)
            {
                player.lifeRegenTime = 0;
                player.lifeRegen -= before;
            }
        }

        public override void UpdateLifeRegen()
        {
            if (glyph == GlyphType.Sanguine)
            {
                player.lifeRegen += 4;
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            // Last hook before player.DashMovement
            DashType dash = FindDashes();
            if (dash != DashType.None)
            {
                // Prevent vanilla dashes
                player.dash = 0;

                if (player.pulley)
                {
                    DashMovement(dash);
                }
            }
        }

        private void DashEnd()
        {
            if (ActiveDash == DashType.Shinigami)
            {
                player.itemAnimation = 0;
            }
        }

        private void DashMovement(DashType dash)
        {
            if (player.dashDelay > 0)
            {
                if (ActiveDash != DashType.None)
                {
                    DashEnd();
                    ActiveDash = DashType.None;
                }
            }
            else if (player.dashDelay < 0)
            {
                // Powered phase
                // Manage dash abilities here
                float speedCap = 12f;
                float decayCapped = 0.992f;
                float speedMax = Math.Max(player.accRunSpeed, player.maxRunSpeed);
                float decayMax = 0.96f;
                int delay = 20;
                if (ActiveDash == DashType.Phase)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        int dust;
                        if (player.velocity.Y == 0f)
                        {
                            dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, mod.DustType("TemporalDust"), 0f, 0f, 100, default, 1.4f);
                        }
                        else
                        {
                            dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height >> 1) - 8f), player.width, 16, mod.DustType("TemporalDust"), 0f, 0f, 100, default, 1.4f);
                        }

                        Main.dust[dust].velocity *= 0.1f;
                        Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
                    }

                    speedCap = speedMax;
                    decayCapped = 0.985f;
                    decayMax = decayCapped;
                    delay = 30;
                }
                else if (ActiveDash == DashType.Firewall)
                {
                    if (firewallHit < 0)
                    {
                        Dust.NewDust(player.position, player.width, player.height, mod.DustType("BinaryDust"));
                        Dust.NewDust(player.position, player.width, player.height, mod.DustType("BinaryDust"));
                        Dust.NewDust(player.position, player.width, player.height, mod.DustType("BinaryDust"));
                        Rectangle hitbox = new Rectangle((int)(player.position.X + player.velocity.X * 0.5 - 4), (int)(player.position.Y + player.velocity.Y * 0.5 - 4), player.width + 8, player.height + 8);
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            var npc = Main.npc[i];
                            if (npc.active && !npc.dontTakeDamage && !npc.friendly)
                            {
                                if (hitbox.Intersects(npc.Hitbox) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height)))
                                {
                                    float damage = 40f * player.meleeDamage;
                                    float knockback = 12f;
                                    bool crit = false;

                                    if (player.kbGlove)
                                    {
                                        knockback *= 2f;
                                    }

                                    if (player.kbBuff)
                                    {
                                        knockback *= 1.5f;
                                    }

                                    if (Main.rand.Next(100) < player.meleeCrit)
                                    {
                                        crit = true;
                                    }

                                    int hitDirection = player.velocity.X < 0f ? -1 : 1;

                                    if (player.whoAmI == Main.myPlayer)
                                    {
                                        npc.AddBuff(mod.BuffType("StackingFireBuff"), 600);
                                        npc.StrikeNPC((int)damage, knockback, hitDirection, crit);
                                        if (Main.netMode != 0)
                                        {
                                            NetMessage.SendData(28, -1, -1, null, i, damage, knockback, hitDirection, 0, 0, 0);
                                        }
                                    }

                                    player.dashDelay = 30;
                                    player.velocity.X = -hitDirection * 1f;
                                    player.velocity.Y = -4f;
                                    player.immune = true;
                                    player.immuneTime = 2;
                                    firewallHit = i;
                                }
                            }
                        }
                    }
                }
                else if (ActiveDash == DashType.Shinigami)
                {
                    speedCap = speedMax;
                    decayCapped = 0.88f;
                    delay = 30;

                    int animationLimit = (int)(player.itemAnimationMax * 0.6f);
                    if (player.itemAnimation > 0 && player.itemAnimation < animationLimit)
                    {
                        player.itemAnimation = animationLimit;
                    }
                }

                if (ActiveDash != DashType.None)
                {
                    if (speedCap < speedMax)
                    {
                        speedCap = speedMax;
                    }

                    player.vortexStealthActive = false;
                    if (player.velocity.X > speedCap || player.velocity.X < -speedCap)
                    {
                        player.velocity.X = player.velocity.X * decayCapped;
                    }
                    else if (player.velocity.X > speedMax || player.velocity.X < -speedMax)
                    {
                        player.velocity.X = player.velocity.X * decayMax;
                    }
                    else
                    {
                        player.dashDelay = delay;

                        if (player.velocity.X < 0f)
                        {
                            player.velocity.X = -speedMax;
                        }
                        else if (player.velocity.X > 0f)
                        {
                            player.velocity.X = speedMax;
                        }
                    }
                }
            }
            else if (dash != DashType.None && player.whoAmI == Main.myPlayer)
            {
                sbyte dir = 0;
                bool dashInput = false;
                if (player.dashTime > 0)
                {
                    player.dashTime--;
                }
                else if (player.dashTime < 0)
                {
                    player.dashTime++;
                }

                if (player.controlRight && player.releaseRight)
                {
                    if (player.dashTime > 0)
                    {
                        dir = 1;
                        dashInput = true;
                        player.dashTime = 0;
                    }
                    else
                    {
                        player.dashTime = 15;
                    }
                }
                else if (player.controlLeft && player.releaseLeft)
                {
                    if (player.dashTime < 0)
                    {
                        dir = -1;
                        dashInput = true;
                        player.dashTime = 0;
                    }
                    else
                    {
                        player.dashTime = -15;
                    }
                }

                if (dashInput)
                {
                    PerformDash(dash, dir);
                }
            }
        }

        internal void PerformDash(DashType dash, sbyte dir, bool local = true)
        {
            float velocity = dir;
            if (dash == DashType.Phase)
            {
                velocity *= 30f;
                phaseStacks--;

                if (local)
                {
                    player.AddBuff(mod.BuffType("TemporalShift"), 3 * 60);
                }

                // vfx
                for (int num17 = 0; num17 < 20; num17++)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, mod.DustType("TemporalDust"), 0f, 0f, 100, default, 2f);
                    Main.dust[dust].position.X += Main.rand.Next(-5, 6);
                    Main.dust[dust].position.Y += Main.rand.Next(-5, 6);
                    Main.dust[dust].velocity *= 0.2f;
                    Main.dust[dust].scale *= 1.4f + Main.rand.Next(20) * 0.01f;
                }
            }
            else if (dash == DashType.Firewall)
            {
                firewallHit = -1;

                Dust.NewDust(player.position, player.width, player.height, mod.DustType("BinaryDust"), 0f, 0f, 0, default, 1f);
                Dust.NewDust(player.position, player.width, player.height, mod.DustType("BinaryDust"), 0f, 0f, 0, default, 1f);

                velocity *= 18.5f;

                for (int num22 = 0; num22 < 0; num22++)
                {
                    int num23f = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, mod.DustType("TemporalDust"), 0f, 0f, 100, default, 2f);
                    Main.dust[num23f].position.X = Main.dust[num23f].position.X + Main.rand.Next(-5, 6);
                    Main.dust[num23f].position.Y = Main.dust[num23f].position.Y + Main.rand.Next(-5, 6);
                    Main.dust[num23f].velocity *= 0.2f;
                    Main.dust[num23f].shader = GameShaders.Armor.GetSecondaryShader(player.shield, player);
                }
            }
            else if (dash == DashType.Shinigami)
            {
                velocity *= 40;
            }

            player.velocity.X = velocity;

            Point feet = (player.Center + new Vector2(dir * (player.width >> 1) + 2, player.gravDir * -player.height * .5f + player.gravDir * 2f)).ToTileCoordinates();
            Point legs = (player.Center + new Vector2(dir * (player.width >> 1) + 2, 0f)).ToTileCoordinates();
            if (WorldGen.SolidOrSlopedTile(feet.X, feet.Y) || WorldGen.SolidOrSlopedTile(legs.X, legs.Y))
            {
                player.velocity.X = player.velocity.X / 2f;
            }
            player.dashDelay = -1;
            ActiveDash = dash;

            if (!local || Main.netMode == 0)
            {
                return;
            }

            ModPacket packet = SpiritMod.instance.GetPacket(MessageType.Dash, 3);
            packet.Write((byte)player.whoAmI);
            packet.Write((byte)dash);
            packet.Write(dir);
            packet.Send();
        }

        public DashType FindDashes()
        {
            if (phaseStacks > 0)
            {
                return DashType.Phase;
            }
            else if (firewall)
            {
                return DashType.Firewall;
            }

            return DashType.None;
        }


        public override void PostUpdateEquips()
        {
            int num323;
            if (graniteSet)
            {
                if (player.controlDown && player.releaseDown)
                {
                    if (player.velocity.Y != 0 && stompCooldown <= 0)
                    {
                        player.AddBuff(mod.BuffType("GraniteBonus"), 300);
                    }
                }
                if (player.velocity.Y > 0 && player.HasBuff(mod.BuffType("GraniteBonus")))
                {
                    player.noFallDmg = true;
                    player.velocity.Y = 15.53f;
                    player.maxFallSpeed = 30f;
                    for (int j = 0; j < 12; j++)
                    {
                        num323 = (int)(player.position.Y / 16f) - player.fallStart;
                        if (num323 >= 16)
                        {
                            num323 = 16;
                        }
                        Vector2 vector2 = Vector2.UnitX;
                        vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(1f * num323, 16f);
                        int num8 = Dust.NewDust(player.Center, 0, 0, 226, 0f, 0f, 160, new Color(), 1f);
                        Main.dust[num8].scale = .68f;
                        Main.dust[num8].noGravity = true;
                        Main.dust[num8].position = player.Center + vector2;
                        Main.dust[num8].velocity = player.velocity * 0.1f;
                        Main.dust[num8].velocity = Vector2.Normalize(player.Center - player.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    }

                    player.armorEffectDrawShadow = true;
                }
            }
            if (Main.mouseRight && magnifyingGlass && Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].damage <= 0)
            {
                player.scope = true;
            }

            if (leatherSet)
            {
                if (concentratedCooldown > 0)
                {
                    if (player.velocity.X != 0f)
                    {
                        concentratedCooldown--;
                    }
                    else
                    {
                        concentratedCooldown -= 2;
                    }
                }
            }
            else
            {
                concentrated = false;
                concentratedCooldown = 360;
            }

            if (concentratedCooldown <= 0)
            {
                concentrated = true;
            }

            if (concentrated)
            {
                Yoraiz0rEye();
            }

            if (clatterboneShield)
            {
                clatterStacks = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
                    {
                        int distance = (int)Main.npc[i].Distance(player.Center);
                        if (distance < 320)
                        {
                            clatterStacks++;
                        }

                        for (int k = 0; k < clatterStacks; k++)
                        {
                            if (Main.rand.NextBool(4))
                            {
                                int d = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 2, 0, 0f, 0f, 100, default, .64f);
                                Main.dust[d].noGravity = true;
                            }
                        }
                    }
                }

                if (clatterStacks >= 5)
                {
                    clatterStacks = 5;
                }
            }
            else
            {
                clatterStacks = 0;
            }

            if (bismiteShield)
            {
                bismiteShieldStacks = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].HasBuff(BuffID.Poisoned) && Main.npc[i].type != NPCID.TargetDummy)
                    {
                        int distance = (int)Main.npc[i].Distance(player.Center);
                        if (distance < 320)
                        {
                            bismiteShieldStacks++;

                        }

                        for (int k = 0; k < bismiteShieldStacks; k++)
                        {
                            if (Main.rand.NextBool(6))
                            {
                                int d = Dust.NewDust(player.position, player.width, player.height, 167, 0f, 0f, 100, default, .45f * bismiteShieldStacks);
                                Main.dust[d].noGravity = true;
                            }
                        }
                    }
                }

                if (bismiteShieldStacks >= 3)
                {
                    bismiteShieldStacks = 3;
                }
            }

            if (frigidGloves)
            {
                frigidGloveStacks = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
                    {
                        int distance = (int)Main.npc[i].Distance(player.Center);
                        if (distance < 320)
                        {
                            frigidGloveStacks++;
                        }

                        for (int k = 0; k < frigidGloveStacks; k++)
                        {
                            if (Main.rand.NextBool(6))
                            {
                                int d = Dust.NewDust(player.position, player.width, player.height, 68, 0f, 0f, 100, default, .45f * bismiteShieldStacks);
                                Main.dust[d].noGravity = true;
                            }
                        }
                    }
                }

                if (frigidGloveStacks >= 5)
                {
                    frigidGloveStacks = 5;
                }
            }

            if (bloodfireShield)
            {
                if (player.lifeRegen >= 0)
                {
                    player.lifeRegen = 0;
                }

                player.lifeRegen--;
                if (player.lifeRegen < 0)
                {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegenCount = 0;

                bloodfireShieldStacks = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
                    {
                        int distance = (int)Main.npc[i].Distance(player.Center);
                        if (distance < 320)
                        {
                            bloodfireShieldStacks++;
                        }

                        for (int k = 0; k < bloodfireShieldStacks; k++)
                        {
                            if (Main.rand.NextBool(6))
                            {
                                int d = Dust.NewDust(player.position, player.width, player.height, 5, 0f, 0f, 0, default, .14f * bloodfireShieldStacks);
                            }
                        }
                    }
                }

                if (bloodfireShieldStacks >= 5)
                {
                    bloodfireShieldStacks = 5;
                }
            }
            else
            {
                bismiteShieldStacks = 0;
            }

            if (player.controlUp && scarabCharm)
            {
                player.AddBuff(BuffID.Featherfall, 30);
            }

            if (frigidSet)
            {
                if (player.controlDown && player.releaseDown && !player.HasBuff(mod.BuffType("FrigidCooldown")))
                {
                    Vector2 mouse = Main.MouseScreen + Main.screenPosition;
                    Projectile.NewProjectile(mouse, Vector2.Zero, mod.ProjectileType("FrigidWall"), 14, 8, player.whoAmI);
                    player.AddBuff(mod.BuffType("FrigidCooldown"), 500);
                }
            }

            //if (glyph == GlyphType.Veil && Math.Abs(player.velocity.X) < 0.05 && Math.Abs(player.velocity.Y) < 0.05)
            //	camoCounter++;
            //else if (camoCounter > 5)
            //	camoCounter -= 5;
            //else
            //	camoCounter = 0;

            if (glyph == GlyphType.Void)
            {
                player.endurance += .08f;
            }
            else if (glyph == GlyphType.Veil)
            {
                //	float camo = (1f / CAMO_DELAY) * camoCounter;
                //	if (camoCounter > CAMO_DELAY)
                //	{
                //		camo = 1f;
                //		camoCounter = CAMO_DELAY;
                //		player.lifeRegen += 3;
                //	}
                //	if (camoCounter > 0 && Main.rand.NextDouble() < camo * .6)
                //	{
                //		player.AddBuff(Buffs.Glyph.PhantomVeil._type, 2);
                //		int dust = Dust.NewDust(player.position, player.width, player.height, 110);
                //		Main.dust[dust].scale = 2.5f * (.75f + .25f * camo);
                //		Main.dust[dust].noGravity = true;
                //	}
                //	player.rangedDamage *= 1 + .15f * camo;
                //	player.magicDamage *= 1 + .15f * camo;
                //	player.thrownDamage *= 1 + .15f * camo;
                //	player.minionDamage *= 1 + .15f * camo;
                //	player.meleeDamage *= 1 + .15f * camo;
            }

            if (phaseShift)
            {
                player.noKnockback = true;
                player.buffImmune[BuffID.Slow] = true;
                player.buffImmune[BuffID.Chilled] = true;
                player.buffImmune[BuffID.Frozen] = true;
                player.buffImmune[BuffID.Webbed] = true;
                player.buffImmune[BuffID.Stoned] = true;
                player.buffImmune[BuffID.OgreSpit] = true;
                player.buffImmune[BuffID.Confused] = true;

                int dust;
                if (player.velocity.Y == 0f)
                {
                    dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, mod.DustType("TemporalDust"), 0f, 0f, 100, default, 1.4f);
                }
                else
                {
                    dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height >> 1) - 8f), player.width, 16, mod.DustType("TemporalDust"), 0f, 0f, 100, default, 1.4f);
                }

                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
            }

            if (clatterboneSet)
            {
                clatterboneTimer--;
            }

            // Update armor sets.
            if (infernalSet)
            {
                int percentageLifeLeft = (int)(player.statLife / player.statLifeMax2 * 100);
                if (percentageLifeLeft <= 25)
                {
                    player.statDefense -= 4;
                    player.manaCost += 0.25F;
                    player.magicDamage += 0.5F;

                    bool spawnProj = true;
                    for (int i = 0; i < 1000; ++i)
                    {
                        if (Main.projectile[i].type == mod.ProjectileType("InfernalGuard") && Main.projectile[i].owner == player.whoAmI)
                        {
                            spawnProj = false;
                            break;
                        }
                    }

                    if (spawnProj)
                    {
                        for (int i = 0; i < 3; ++i)
                        {
                            int newProj = Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("InfernalGuard"), 0, 0, player.whoAmI, 90, 1);
                            Main.projectile[newProj].localAI[1] = 2f * (float)Math.PI / 3f * i;
                        }
                    }

                    player.AddBuff(mod.BuffType("InfernalRage"), 2);
                    infernalSetCooldown = 60;
                }
            }

            if (infernalSetCooldown > 0)
            {
                infernalSetCooldown--;
            }

            if (runicSet)
            {
                SpawnRunicRunes();
            }

            if (spiritSet)
            {
                if (Main.rand.NextBool(5))
                {
                    int num = Dust.NewDust(player.position, player.width, player.height, 261, 0f, 0f, 0, default, 1f);
                    Main.dust[num].noGravity = true;
                }

                if (player.statLife >= 400)
                {
                    player.meleeDamage += 0.08f;
                    player.magicDamage += 0.08f;
                    player.minionDamage += 0.08f;
                    player.thrownDamage += 0.08f;
                    player.rangedDamage += 0.08f;
                }
                else if (player.statLife >= 200)
                {
                    player.statDefense += 6;
                }
                else if (player.statLife >= 50)
                {
                    player.lifeRegenTime += 5;
                }
                else if (player.statLife > 0)
                {
                    player.noKnockback = true;
                }
            }

            if (bloomwindSet)
            {
                player.AddBuff(mod.BuffType("BloomwindMinionBuff"), 3600);
                timer1++;
                if (player.ownedProjectileCounts[mod.ProjectileType("BloomwindMinion")] <= 0 && timer1 > 30)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("BloomwindMinion"), 35, 0, player.whoAmI);
                    timer1 = 0;
                }
            }

            if (Ward || Ward1)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("WardProj")] <= 1)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("WardProj"), 0, 0, player.whoAmI);
                }
            }
            if (cryoSet)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("CryoProj")] <= 1)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("CryoProj"), 0, 0, player.whoAmI);
                }
            }
            if (atmos)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("AtmosProj")] <= 1)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("AtmosProj"), 0, 0, player.whoAmI);
                }
            }

            if (oceanSet)
            {
                timerz++;
                player.AddBuff(mod.BuffType("BabyClamperBuff"), 3600);
                if (player.ownedProjectileCounts[mod.ProjectileType("BabyClamper")] <= 0 && timerz > 30)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("BabyClamper"), 19, 0, player.whoAmI);
                    timerz = 0;
                }
            }

            if (animusLens)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("ShadowGuard")] <= 0)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("ShadowGuard"), 20, 0, player.whoAmI);
                }

                if (player.ownedProjectileCounts[mod.ProjectileType("SpiritGuard")] <= 0)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("SpiritGuard"), 20, 0, player.whoAmI);
                }
            }

            if (witherSet)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("WitherOrb")] <= 0)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("WitherOrb"), 45, 0, player.whoAmI);
                }
            }

            Counter++;
            if (MoonSongBlossom)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("MoonShard")] <= 2 && Counter > 120)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("MoonShard"), 25, 0, player.whoAmI);
                    Counter = 0;
                }
            }

            if (crystalSet)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("HedronCrystal")] <= 1)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("HedronCrystal"), 25, 0, player.whoAmI);
                }
            }

            if (illuminantSet && (Main.rand.NextBool(12)))
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("EnchantedSword")] <= 3)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("EnchantedSword"), 28, 0, player.whoAmI);
                }
            }

            if (shadowSet && (Main.rand.NextBool(2)))
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("Spirit")] <= 2)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("Spirit"), 56, 0, player.whoAmI);
                }
            }

            if (SoulStone && (Main.rand.NextBool(2)))
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("StoneSpirit")] < 1)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("StoneSpirit"), 35, 0, player.whoAmI);
                }
            }

            if (duskSet)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType("ShadowCircleRune1")] <= 0)
                {
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("ShadowCircleRune1"), 18, 0, player.whoAmI);
                }
            }


            if (shadowSet)
            {
                if (infernalDash > 0)
                {
                    infernalDash--;
                }
                else
                {
                    infernalHit = -1;
                }

                if (infernalDash > 0 && infernalHit < 0)
                {
                    Rectangle rectangle = new Rectangle((int)(player.position.X + player.velocity.X * 0.5 - 4.0), (int)(player.position.Y + player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                        {
                            NPC npc = Main.npc[i];
                            Rectangle rect = npc.getRect();
                            if (rectangle.Intersects(rect) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height)))
                            {
                                float damage = 100f * player.meleeDamage;

                                float knockback = 10f;
                                if (player.kbGlove)
                                {
                                    knockback *= 0f;
                                }

                                if (player.kbBuff)
                                {
                                    knockback *= 1f;
                                }

                                bool crit = false;
                                if (Main.rand.Next(100) < player.meleeCrit)
                                {
                                    crit = true;
                                }

                                int hitDirection = player.direction;
                                if (player.velocity.X < 0f)
                                {
                                    hitDirection = -1;
                                }

                                if (player.velocity.X > 0f)
                                {
                                    hitDirection = 1;
                                }

                                if (player.whoAmI == Main.myPlayer)
                                {
                                    npc.AddBuff(mod.BuffType("SoulFlare"), 600);
                                    npc.StrikeNPC((int)damage, knockback, hitDirection, crit, false, false);

                                    if (Main.netMode != 0)
                                    {
                                        NetMessage.SendData(28, -1, -1, null, i, damage, knockback, hitDirection);
                                    }
                                }

                                infernalDash = 10;
                                player.dashDelay = 0;
                                player.velocity.X = -hitDirection * 2f;
                                player.velocity.Y = -2f;
                                player.immune = true;
                                player.immuneTime = 7;
                                infernalHit = i;
                            }
                        }
                    }
                }

                if (player.dash <= 0 && player.dashDelay == 0 && !player.mount.Active)
                {
                    int num21 = 0;
                    bool flag2 = false;

                    if (player.dashTime > 0)
                    {
                        player.dashTime--;
                    }

                    if (player.dashTime < 0)
                    {
                        player.dashTime++;
                    }

                    if (player.controlRight && player.releaseRight)
                    {
                        if (player.dashTime > 0)
                        {
                            num21 = 1;
                            flag2 = true;
                            player.dashTime = 0;
                        }
                        else
                        {
                            player.dashTime = 15;
                        }
                    }
                    else if (player.controlLeft && player.releaseLeft)
                    {
                        if (player.dashTime < 0)
                        {
                            num21 = -1;
                            flag2 = true;
                            player.dashTime = 0;
                        }
                        else
                        {
                            player.dashTime = -15;
                        }
                    }

                    if (flag2)
                    {
                        player.velocity.X = 15.5f * num21;
                        Point point3 = (player.Center + new Vector2(num21 * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
                        Point point4 = (player.Center + new Vector2(num21 * player.width / 2 + 2, 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y))
                        {
                            player.velocity.X = player.velocity.X / 2f;
                        }

                        player.dashDelay = -1;
                        infernalDash = 15;

                        for (int num22 = 0; num22 < 0; num22++)
                        {
                            int num23 = Dust.NewDust(player.position, player.width, player.height, 31, 0f, 0f, 100, default, 2f);
                            Main.dust[num23].position.X = Main.dust[num23].position.X + Main.rand.Next(-5, 6);
                            Main.dust[num23].position.Y = Main.dust[num23].position.Y + Main.rand.Next(-5, 6);
                            Main.dust[num23].velocity *= 0.2f;
                            Main.dust[num23].scale *= 1f + Main.rand.Next(20) * 0.01f;
                            Main.dust[num23].shader = GameShaders.Armor.GetSecondaryShader(player.shield, player);
                        }
                    }
                }
            }

            if (infernalDash > 0)
            {
                infernalDash--;
            }

            if (player.dashDelay < 0)
            {
                for (int l = 0; l < 0; l++)
                {
                    int num14;
                    if (player.velocity.Y == 0f)
                    {
                        num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, 31, 0f, 0f, 100, default, 1.4f);
                    }
                    else
                    {
                        num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height / 2) - 8f), player.width, 16, 31, 0f, 0f, 100, default, 1.4f);
                    }
                    Main.dust[num14].velocity *= 0.1f;
                    Main.dust[num14].scale *= 1f + Main.rand.Next(20) * 0.01f;
                    Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(player.shoe, player);

                    int dust = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                    Main.dust[dust].scale *= 10f;

                    int dust2 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                    Main.dust[dust2].scale *= 10f;

                    int dust3 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                    Main.dust[dust3].scale *= 10f;
                }

                player.vortexStealthActive = false;

                float maxSpeed = Math.Max(player.accRunSpeed, player.maxRunSpeed);
                if (player.velocity.X > 12f || player.velocity.X < -12f)
                {
                    player.velocity.X = player.velocity.X * 0.985f;
                    return;
                }

                if (player.velocity.X > maxSpeed || player.velocity.X < -maxSpeed)
                {
                    player.velocity.X = player.velocity.X * 0.94f;
                    return;
                }

                player.dashDelay = 30;

                if (player.velocity.X < 0f)
                {
                    player.velocity.X = -maxSpeed;
                    return;
                }

                if (player.velocity.X > 0f)
                {
                    player.velocity.X = maxSpeed;
                    return;
                }
            }

            // Update accessories.
            if (infernalShield)
            {
                if (infernalDash > 0)
                {
                    infernalDash--;
                }
                else
                {
                    infernalHit = -1;
                }

                if (infernalDash > 0 && infernalHit < 0)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                    Main.dust[dust].scale *= 2f;

                    int dust2 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                    Main.dust[dust2].scale *= 2f;

                    int dust3 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                    Main.dust[dust3].scale *= 2f;

                    Rectangle rectangle = new Rectangle((int)(player.position.X + player.velocity.X * 0.5 - 4.0), (int)(player.position.Y + player.velocity.Y * 0.5 - 4.0), player.width + 8, player.height + 8);
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                        {
                            NPC npc = Main.npc[i];
                            Rectangle rect = npc.getRect();
                            if (rectangle.Intersects(rect) && (npc.noTileCollide || Collision.CanHit(player.position, player.width, player.height, npc.position, npc.width, npc.height)))
                            {
                                float damage = 30f * player.meleeDamage;

                                float knockback = 12f;
                                if (player.kbGlove)
                                {
                                    knockback *= 2f;
                                }

                                if (player.kbBuff)
                                {
                                    knockback *= 1.5f;
                                }

                                bool crit = false;
                                if (Main.rand.Next(100) < player.meleeCrit)
                                {
                                    crit = true;
                                }

                                int hitDirection = player.direction;
                                if (player.velocity.X < 0f)
                                {
                                    hitDirection = -1;
                                }

                                if (player.velocity.X > 0f)
                                {
                                    hitDirection = 1;
                                }

                                if (player.whoAmI == Main.myPlayer)
                                {
                                    npc.AddBuff(mod.BuffType("StackingFireBuff"), 600);
                                    npc.StrikeNPC((int)damage, knockback, hitDirection, crit, false, false);

                                    if (Main.netMode != 0)
                                    {
                                        NetMessage.SendData(28, -1, -1, null, i, damage, knockback, hitDirection, 0, 0, 0);
                                    }
                                }

                                infernalDash = 10;
                                player.dashDelay = 30;
                                player.velocity.X = -hitDirection * 1f;
                                player.velocity.Y = -4f;
                                player.immune = true;
                                player.immuneTime = 2;
                                infernalHit = i;
                            }
                        }
                    }
                }

                if (player.dash <= 0 && player.dashDelay == 0 && !player.mount.Active)
                {
                    int num21 = 0;
                    bool flag2 = false;

                    if (player.dashTime > 0)
                    {
                        player.dashTime--;
                    }

                    if (player.dashTime < 0)
                    {
                        player.dashTime++;
                    }

                    if (player.controlRight && player.releaseRight)
                    {
                        if (player.dashTime > 0)
                        {
                            num21 = 1;
                            flag2 = true;
                            player.dashTime = 0;
                        }
                        else
                        {
                            player.dashTime = 15;
                        }
                    }
                    else if (player.controlLeft && player.releaseLeft)
                    {
                        if (player.dashTime < 0)
                        {
                            num21 = -1;
                            flag2 = true;
                            player.dashTime = 0;
                        }
                        else
                        {
                            player.dashTime = -15;
                        }
                    }

                    if (flag2)
                    {
                        int dust = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].scale *= 2f;

                        int dust2 = Dust.NewDust(player.position, player.width, player.height, 6, 0f, 0f, 0, default, 1f);
                        Main.dust[dust2].scale *= 2f;

                        player.velocity.X = 15.5f * num21;

                        Point point3 = (player.Center + new Vector2(num21 * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
                        Point point4 = (player.Center + new Vector2(num21 * player.width / 2 + 2, 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y))
                        {
                            player.velocity.X = player.velocity.X / 2f;
                        }

                        player.dashDelay = -1;
                        infernalDash = 15;

                        for (int num22 = 0; num22 < 0; num22++)
                        {
                            int num23 = Dust.NewDust(player.position, player.width, player.height, 31, 0f, 0f, 100, default, 2f);
                            Main.dust[num23].position.X = Main.dust[num23].position.X + Main.rand.Next(-5, 6);
                            Main.dust[num23].position.Y = Main.dust[num23].position.Y + Main.rand.Next(-5, 6);
                            Main.dust[num23].velocity *= 0.2f;
                            Main.dust[num23].scale *= 1f + Main.rand.Next(20) * 0.01f;
                            Main.dust[num23].shader = GameShaders.Armor.GetSecondaryShader(player.shield, player);
                        }
                    }
                }
            }

            if (infernalDash > 0)
            {
                infernalDash--;
            }

            if (player.dashDelay < 0)
            {
                for (int l = 0; l < 0; l++)
                {
                    int num14;
                    if (player.velocity.Y == 0f)
                    {
                        num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, 31, 0f, 0f, 100, default, 1.4f);
                    }
                    else
                    {
                        num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (player.height / 2) - 8f), player.width, 16, 31, 0f, 0f, 100, default, 1.4f);
                    }
                    Main.dust[num14].velocity *= 0.1f;
                    Main.dust[num14].scale *= 1f + Main.rand.Next(20) * 0.01f;
                    Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(player.shoe, player);
                }

                player.vortexStealthActive = false;

                float maxSpeed = Math.Max(player.accRunSpeed, player.maxRunSpeed);
                if (player.velocity.X > 12f || player.velocity.X < -12f)
                {
                    player.velocity.X = player.velocity.X * 0.985f;
                    return;
                }

                if (player.velocity.X > maxSpeed || player.velocity.X < -maxSpeed)
                {
                    player.velocity.X = player.velocity.X * 0.94f;
                    return;
                }

                player.dashDelay = 30;

                if (player.velocity.X < 0f)
                {
                    player.velocity.X = -maxSpeed;
                    return;
                }

                if (player.velocity.X > 0f)
                {
                    player.velocity.X = maxSpeed;
                    return;
                }
            }

            if (shadowGauntlet)
            {
                player.kbGlove = true;
                player.meleeDamage += 0.07F;
                player.meleeSpeed += 0.07F;
            }

            if (goldenApple)
            {
                int num2 = 20;
                float num3 = (player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * num2;
                player.statDefense += (int)num3;
            }

            if (bubbleTimer > 0)
            {
                bubbleTimer--;
            }

            if (soulSiphon > 0)
            {
                player.lifeRegenTime += 2;

                int num = (5 + soulSiphon) / 2;
                player.lifeRegenTime += num;
                player.lifeRegen += num;

                soulSiphon = 0;
            }

            if (drakomireMount)
            {
                player.statDefense += 40;
                player.noKnockback = true;

                if (player.dashDelay > 0)
                {
                    player.dashDelay--;
                }
                else
                {
                    int num4 = 0;
                    bool flag = false;

                    if (player.dashTime > 0)
                    {
                        player.dashTime--;
                    }
                    else if (player.dashTime < 0)
                    {
                        player.dashTime++;
                    }

                    if (player.controlRight && player.releaseRight)
                    {
                        if (player.dashTime > 0)
                        {
                            num4 = 1;
                            flag = true;
                            player.dashTime = 0;
                        }
                        else
                        {
                            player.dashTime = 15;
                        }
                    }
                    else if (player.controlLeft && player.releaseLeft)
                    {
                        if (player.dashTime < 0)
                        {
                            num4 = -1;
                            flag = true;
                            player.dashTime = 0;
                        }
                        else
                        {
                            player.dashTime = -15;
                        }
                    }

                    if (flag)
                    {
                        player.velocity.X = 16.9f * num4;
                        Point point = Utils.ToTileCoordinates(player.Center + new Vector2(num4 * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f));
                        Point point2 = Utils.ToTileCoordinates(player.Center + new Vector2(num4 * player.width / 2 + 2, 0f));
                        if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                        {
                            player.velocity.X = player.velocity.X / 2f;
                        }

                        player.dashDelay = 600;
                    }
                }

                if (player.velocity.X != 0f && player.velocity.Y == 0f)
                {
                    drakomireFlameTimer += (int)Math.Abs(player.velocity.X);
                    if (drakomireFlameTimer >= 15)
                    {
                        Vector2 vector = player.Center + new Vector2(26 * -(float)player.direction, 26f * player.gravDir);
                        Projectile.NewProjectile(vector.X, vector.Y, 0f, 0f, mod.ProjectileType("DrakomireFlame"), player.statDefense / 2, 0f, player.whoAmI, 0f, 0f);
                        drakomireFlameTimer = 0;
                    }
                }

                if (Main.rand.Next(10) == 0)
                {
                    Vector2 vector2 = player.Center + new Vector2(-48 * player.direction, -6f * player.gravDir);
                    if (player.direction == -1)
                    {
                        vector2.X -= 20f;
                    }

                    Dust.NewDust(vector2, 16, 16, 6, 0f, 0f, 0, default, 1f);
                }
            }
        }

        public override void PostUpdateRunSpeeds()
        {
            if (copterBrake && player.mount.Active && player.mount.Type == mod.MountType("CandyCopter"))
            {
                // Prevent horizontal movement
                player.maxRunSpeed = 0f;
                player.runAcceleration = 0f;

                // Deplete horizontal velocity
                if (player.velocity.X > CandyCopter.groundSlowdown)
                {
                    player.velocity.X -= CandyCopter.groundSlowdown;
                }
                else if (player.velocity.X < -CandyCopter.groundSlowdown)
                {
                    player.velocity.X += CandyCopter.groundSlowdown;
                }
                else
                {
                    player.velocity.X = 0f;
                }

                // Prevent further depletion by game engine
                player.runSlowdown = 0f;
            }

            // Adjust speed here to also affect mounted speed.
            float speed = 1f;
            float sprint = 1f;
            float accel = 1f;
            float slowdown = 1f;

            if (glyph == GlyphType.Frost)
            {
                sprint += .05f;
            }

            if (phaseShift)
            {
                speed += 0.55f;
                sprint += 0.55f;
                accel += 3f;
                slowdown += 3f;
            }

            player.maxRunSpeed *= speed;
            player.accRunSpeed *= sprint;
            player.runAcceleration *= accel;
            player.runSlowdown *= slowdown;

            DashMovement(FindDashes());
        }

        public override void PostUpdate()
        {
            if (cryoSet)
            {
                cryoTimer += .5f;
                if (cryoTimer >= 450)
                {
                    cryoTimer = 450;
                }
            }
            else
            {
                cryoTimer = 0;
            }
            if (bismiteSet)
            {
                if (player.HasBuff(mod.BuffType("VirulenceCooldown")) || virulence >= 0)
                {
                    virulence--;
                }

                if (virulence == 0f)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
                    Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
                    CombatText.NewText(textPos, new Color(95, 156, 111, 100), "Virulence Charged!");
                }
            }

            if (daybloomSet)
            {
                if (dazzleStacks == 3600)
                {
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
                    Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
                    CombatText.NewText(textPos, new Color(245, 212, 69, 100), "Energy Charged!");
                }

                if (dazzleStacks >= 3600)
                {
                    if (Main.rand.Next(6) == 0)
                    {
                        int d = Dust.NewDust(player.position, player.width, player.height, 228, 0f, 0f, 0, default, .14f * bloodfireShieldStacks);
                    }
                }
            }

            if (shootDelay > 0)
            {
                shootDelay--;

                Rectangle rect = new Rectangle((int)player.Center.X, (int)player.position.Y, 1, -1);
                float x = Main.rand.NextFloat() * rect.Width;
                float y = Main.rand.NextFloat() * rect.Height;

                Tile atTile = Framing.GetTileSafely((int)((rect.X + x) / 16), (int)((rect.Y + y) / 16));
                if (!atTile.active())
                {
                    Dust.NewDust(new Vector2(rect.X + x, rect.Y + y), 2, 6, 6);
                }

                Dust.NewDust(new Vector2(rect.X + x, rect.Y + y), 2, 6, 244);
                Dust.NewDust(new Vector2(rect.X + x, rect.Y + y), 2, 6, 244);
                Dust.NewDust(new Vector2(rect.X + x, rect.Y + y), 2, 6, 6);
            }

            if (shootDelay1 > 0)
            {
                shootDelay1--;
            }

            if (shootDelay2 > 0)
            {
                shootDelay2--;
            }

            if (shootDelay3 > 0)
            {
                shootDelay3--;
            }
        }


        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (CursedPendant && Main.rand.NextBool(5))
            {
                target.AddBuff(BuffID.CursedInferno, 180);
            }

            if (IchorPendant && Main.rand.NextBool(10))
            {
                target.AddBuff(BuffID.Ichor, 180);
            }

            if (shadowGauntlet && Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.ShadowFlame, 180);
            }
            if (twilightTalisman && Main.rand.NextBool(15))
            {
                target.AddBuff(BuffID.ShadowFlame, 180);
            }
            if (duskSet && item.magic && Main.rand.NextBool(4))
            {
                target.AddBuff(BuffID.ShadowFlame, 300);
            }

            if (primalSet && item.melee && Main.rand.NextBool(2))
            {
                target.AddBuff(mod.BuffType("Afflicted"), 120);
            }

            if (illuminantSet && item.melee && Main.rand.NextBool(4))
            {
                target.AddBuff(mod.BuffType("HolyLight"), 120);
            }

            if (moonGauntlet && item.melee)
            {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.CursedInferno, 180);
                }

                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Ichor, 180);
                }

                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Daybreak, 180);
                }

                if (Main.rand.NextBool(8))
                {
                    player.AddBuff(mod.BuffType("OnyxWind"), 120);
                }
            }

            if (Ward1 && crit)
            {
                if (Main.rand.NextBool(10))
                {
                    player.statLife += 2;
                }

                player.HealEffect(2);
            }

            if (starBuff && crit)
            {
                if (Main.rand.NextBool(10))
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        if (Main.myPlayer == player.whoAmI)
                        {
                            Projectile.NewProjectile(target.Center.X + Main.rand.Next(-140, 140), target.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), 92, 40, 3, player.whoAmI);
                        }
                    }
                }
            }

            if (poisonPotion && crit)
            {
                if (Main.rand.NextBool(10))
                {
                    target.AddBuff(BuffID.Poisoned, 180);
                }
            }

            if (runeBuff && item.magic)
            {
                if (Main.rand.NextBool(10))
                {
                    for (int h = 0; h < 3; h++)
                    {
                        Vector2 vel = new Vector2(0, -1);
                        float rand = Main.rand.NextFloat() * 6.283f;
                        vel = vel.RotatedBy(rand);
                        vel *= 8f;
                        Projectile.NewProjectile(target.Center - new Vector2(10f, 10f), vel, mod.ProjectileType("Rune"), 27, 1, player.whoAmI);
                    }
                }
            }

            if (moonHeart && Main.rand.NextBool(12))
            {
                if (item.melee || item.ranged || item.magic || item.thrown)
                {
                    player.AddBuff(mod.BuffType("CelestialWill"), 300);
                }
            }

            if (concentrated)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dust = Dust.NewDust(target.Center, target.width, target.height, DustID.GoldCoin);
                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].noGravity = true;

                    Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    vector2_1.Normalize();

                    Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();

                    Vector2 vector2_3 = vector2_2 * 34f;
                    Main.dust[dust].position = target.Center - vector2_3;
                }

                damage = (int)(damage * 1.2F);
                crit = true;
                concentrated = false;
                concentratedCooldown = 300;
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (icySoul && Main.rand.NextBool(6))
            {
                if (proj.thrown)
                {
                    target.AddBuff(mod.BuffType("SoulBurn"), 280);
                }

                if (proj.magic)
                {
                    target.AddBuff(BuffID.Frostburn, 280);
                }
            }
            if (twilightTalisman && Main.rand.NextBool(15))
            {
                target.AddBuff(BuffID.ShadowFlame, 180);
            }
            if (shadowGauntlet && proj.melee && Main.rand.NextBool(2))
            {
                target.AddBuff(BuffID.ShadowFlame, 180);
            }

            if (poisonPotion && crit && Main.rand.NextBool(10))
            {
                target.AddBuff(BuffID.Poisoned, 180);
            }

            if (moonGauntlet && proj.melee)
            {
                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.CursedInferno, 180);
                }

                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Ichor, 180);
                }

                if (Main.rand.NextBool(4))
                {
                    target.AddBuff(BuffID.Daybreak, 180);
                }

                if (Main.rand.NextBool(8))
                {
                    player.AddBuff(mod.BuffType("OnyxWind"), 120);
                }
            }

            if (acidSet && proj.thrown && Main.rand.NextBool(100))
            {
                target.AddBuff(mod.BuffType("Death"), 60);
            }

            if (runeBuff && proj.magic && Main.rand.NextBool(10))
            {
                for (int h = 0; h < 3; h++)
                {
                    Vector2 vel = new Vector2(0, -1);
                    float rand = Main.rand.NextFloat() * 6.283f;
                    vel = vel.RotatedBy(rand);
                    vel *= 8f;
                    Projectile.NewProjectile(target.Center.X - 10, target.Center.Y - 10, vel.X, vel.Y, mod.ProjectileType("Rune"), 27, 1, player.whoAmI);
                }
            }

            if (starBuff && crit && Main.rand.NextBool(10))
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (Main.myPlayer == player.whoAmI)
                    {
                        Projectile.NewProjectile(target.Center.X + Main.rand.Next(-140, 140), target.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), 92, 40, 3, player.whoAmI);
                    }
                }
            }

            if (illuminantSet && Main.rand.NextBool(4))
            {
                if (proj.melee || proj.ranged || proj.magic || proj.thrown)
                {
                    target.AddBuff(mod.BuffType("HolyLight"), 120);
                }
            }

            if (moonHeart && Main.rand.NextBool(15))
            {
                player.AddBuff(mod.BuffType("CelestialWill"), 300);
            }

            if (primalSet && Main.rand.NextBool(2))
            {
                if (proj.magic || proj.melee)
                {
                    target.AddBuff(mod.BuffType("Afflicted"), 120);
                }
            }

            if (duskSet && proj.magic && Main.rand.NextBool(4))
            {
                target.AddBuff(BuffID.ShadowFlame, 300);
            }

            if (Ward1 && crit && Main.rand.NextBool(10))
            {
                player.statLife += 2;
                player.HealEffect(2);
            }

            if (concentrated)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dust = Dust.NewDust(target.Center, target.width, target.height, DustID.GoldCoin);
                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].noGravity = true;

                    Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    vector2_1.Normalize();

                    Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();

                    Vector2 vector2_3 = vector2_2 * 34f;
                    Main.dust[dust].position = target.Center - vector2_3;
                }

                damage = (int)(damage * 1.2F);
                crit = true;
                concentrated = false;
                concentratedCooldown = 300;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (npc.whoAmI == infernalHit)
            {
                damage = 0;
            }
            if (coralSet)
            {
                for (int k = 0; k < 10; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 225, 2.5f, -2.5f, 0, Color.White, 0.7f);
                    Dust.NewDust(npc.position, npc.width, npc.height, 225, 2.5f, -2.5f, 0, default(Color), .34f);
                }
                npc.StrikeNPC(damage / 3, 1f, 0, crit);
            }
        }

        public void Yoraiz0rEye()
        {
            int index = 0 + player.bodyFrame.Y / 56;
            if (index >= Main.OffsetsPlayerHeadgear.Length)
            {
                index = 0;
            }

            Vector2 vector2_1 = Vector2.Zero;
            if (player.mount.Active && player.mount.Cart)
            {
                int num = Math.Sign(player.velocity.X);
                if (num == 0)
                {
                    num = player.direction;
                }

                vector2_1 = new Vector2(MathHelper.Lerp(0.0f, -8f, player.fullRotation / 0.7853982f), MathHelper.Lerp(0.0f, 2f, Math.Abs(player.fullRotation / 0.7853982f))).RotatedBy(player.fullRotation, new Vector2());
                if (num == Math.Sign(player.fullRotation))
                {
                    vector2_1 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / 0.7853982f));
                }
            }

            Vector2 spinningpoint1 = new Vector2(3 * player.direction - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir) + Vector2.UnitY * player.gfxOffY + player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
            Vector2 spinningpoint2 = new Vector2(3 * player.shadowDirection[1] - (player.direction == 1 ? 1 : 0), -11.5f * player.gravDir) + player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
            if (player.fullRotation != 0.0)
            {
                spinningpoint1 = spinningpoint1.RotatedBy(player.fullRotation, player.fullRotationOrigin);
                spinningpoint2 = spinningpoint2.RotatedBy(player.fullRotation, player.fullRotationOrigin);
            }

            float num1 = 0.0f;
            if (player.mount.Active)
            {
                num1 = player.mount.PlayerOffset;
            }

            Vector2 vector2_2 = player.position + spinningpoint1 + vector2_1;
            vector2_2.Y -= num1 / 2f;

            Vector2 vector2_3 = player.oldPosition + spinningpoint2 + vector2_1;
            vector2_3.Y -= num1 / 2f;

            float num2 = 1f;
            switch (player.yoraiz0rEye % 10)
            {
                case 1:
                    return;

                case 2:
                    num2 = 0.35f;
                    break;

                case 3:
                    num2 = 0.425f;
                    break;

                case 4:
                    num2 = 0.5f;
                    break;

                case 5:
                    num2 = 0.75f;
                    break;

                case 6:
                    num2 = .85f;
                    break;

                case 7:
                    num2 = 1f;
                    break;
            }

            if (player.yoraiz0rEye < 7)
            {
                DelegateMethods.v3_1 = Main.hslToRgb(Main.rgbToHsl(player.eyeColor).X, 1f, 0.5f).ToVector3() * 0.5f * num2;
                if (player.velocity != Vector2.Zero)
                {
                    Utils.PlotTileLine(player.Center, player.Center + player.velocity * 2f, 4f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
                }
                else
                {
                    Utils.PlotTileLine(player.Left, player.Right, 4f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
                }
            }

            int num3 = (int)Vector2.Distance(vector2_2, vector2_3) / 3 + 1;
            if (Vector2.Distance(vector2_2, vector2_3) % 3.0 != 0.0)
            {
                ++num3;
            }

            for (float num4 = 1f; num4 <= (double)num3; ++num4)
            {
                Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f)];
                dust.position = Vector2.Lerp(vector2_3, vector2_2, num4 / num3);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.customData = player;
                dust.scale = num2;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cYorai, player);
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (chitinSet)
            {
                if (player.velocity.X != 0)
                {
                    int knockBack = 9;
                    int dam = 18;

                    int hitDirection = player.direction;
                    if (player.velocity.X < 0f)
                    {
                        hitDirection = -1;
                    }

                    if (player.velocity.X > 0f)
                    {
                        hitDirection = 1;
                    }

                    npc.StrikeNPCNoInteraction(dam, knockBack, -hitDirection, false, false, false);

                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, mod.ProjectileType("ScarabProjectile"), dam/2, knockBack, 0, Main.myPlayer);
                    Main.projectile[p].melee = false;
                }
            }

            if (bismiteShield)
            {
                npc.AddBuff(BuffID.Poisoned, 300);
            }

            if (basiliskMount)
            {
                int num = player.statDefense / 2;
                npc.StrikeNPCNoInteraction(num, 0f, 0, false, false, false);
            }
        }

        internal bool CanTrickOrTreat(NPC npc)
        {
            if (!npc.townNPC)
            {
                return false;
            }

            string fullName;
            if (npc.modNPC == null)
            {
                fullName = "Terraria:" + npc.TypeName;
            }
            else
            {
                fullName = npc.modNPC.mod.Name + ":" + npc.TypeName;
            }

            if (candyFromTown.Contains(fullName))
            {
                return false;
            }

            candyFromTown.Add(fullName);
            return true;
        }

        private void SpawnRunicRunes()
        {
            if (Main.rand.Next(15) == 0)
            {
                int runeAmount = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == mod.ProjectileType("RunicRune"))
                    {
                        runeAmount++;
                    }
                }

                if (Main.rand.Next(15) >= runeAmount && runeAmount < 10)
                {
                    int dimension = 24;
                    int dimension2 = 90;
                    for (int j = 0; j < 50; j++)
                    {
                        int randValue = Main.rand.Next(200 - j * 2, 400 + j * 2);
                        Vector2 center = player.Center;
                        center.X += Main.rand.Next(-randValue, randValue + 1);
                        center.Y += Main.rand.Next(-randValue, randValue + 1);

                        if (!Collision.SolidCollision(center, dimension, dimension) && !Collision.WetCollision(center, dimension, dimension))
                        {
                            center.X += dimension / 2;
                            center.Y += dimension / 2;

                            if (Collision.CanHit(new Vector2(player.Center.X, player.position.Y), 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(player.Center.X, player.position.Y - 50f), 1, 1, center, 1, 1))
                            {
                                int x = (int)center.X / 16;
                                int y = (int)center.Y / 16;

                                bool flag = false;
                                if (Main.rand.Next(4) == 0 && Main.tile[x, y] != null && Main.tile[x, y].wall > 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    center.X -= dimension2 / 2;
                                    center.Y -= dimension2 / 2;

                                    if (Collision.SolidCollision(center, dimension2, dimension2))
                                    {
                                        center.X += dimension2 / 2;
                                        center.Y += dimension2 / 2;
                                        flag = true;
                                    }
                                }

                                if (flag)
                                {
                                    for (int k = 0; k < 1000; k++)
                                    {
                                        if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].type == mod.ProjectileType("RunicRune") && (center - Main.projectile[k].Center).Length() < 48f)
                                        {
                                            flag = false;
                                            break;
                                        }
                                    }

                                    if (flag && Main.myPlayer == player.whoAmI)
                                    {
                                        Projectile.NewProjectile(center.X, center.Y, 0f, 0f, mod.ProjectileType("RunicRune"), 40, 1.5f, player.whoAmI, 0f, 0f);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private Vector2 TestTeleport(ref bool canSpawn, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY)
        {
            Player player = Main.player[Main.myPlayer];

            int num1 = 0;
            int num2 = 0;
            int num3 = 0;

            Vector2 Position = new Vector2(num2, num3) * 16f + new Vector2(-player.width / 2 + 8, -player.height);
            while (!canSpawn && num1 < 1000)
            {
                ++num1;

                int index1 = teleportStartX + Main.rand.Next(teleportRangeX);
                int index2 = teleportStartY + Main.rand.Next(teleportRangeY);
                Position = new Vector2(index1, index2) * 16f + new Vector2(-player.width / 2 + 8, -player.height);

                if (!Collision.SolidCollision(Position, player.width, player.height))
                {
                    if (Main.tile[index1, index2] == null)
                    {
                        Main.tile[index1, index2] = new Tile();
                    }

                    if ((Main.tile[index1, index2].wall != 87 || index2 <= Main.worldSurface || NPC.downedPlantBoss) && (!Main.wallDungeon[Main.tile[index1, index2].wall] || index2 <= Main.worldSurface || NPC.downedBoss3))
                    {
                        int num4 = 0;
                        while (num4 < 100)
                        {
                            if (Main.tile[index1, index2 + num4] == null)
                            {
                                Main.tile[index1, index2 + num4] = new Tile();
                            }

                            Tile tile = Main.tile[index1, index2 + num4];
                            Position = new Vector2(index1, index2 + num4) * 16f + new Vector2(-player.width / 2 + 8, -player.height);
                            Vector4 vector4 = Collision.SlopeCollision(Position, player.velocity, player.width, player.height, player.gravDir, false);

                            bool flag = !Collision.SolidCollision(Position, player.width, player.height);
                            if (vector4.Z == (double)player.velocity.X)
                            {
                                double y = player.velocity.Y;
                            }

                            if (flag)
                            {
                                ++num4;
                            }
                            else if (!tile.active() || tile.inActive() || !Main.tileSolid[tile.type])
                            {
                                ++num4;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (!Collision.LavaCollision(Position, player.width, player.height) && Collision.HurtTiles(Position, player.velocity, player.width, player.height, false).Y <= 0.0)
                        {
                            Collision.SlopeCollision(Position, player.velocity, player.width, player.height, player.gravDir, false);
                            if (Collision.SolidCollision(Position, player.width, player.height) && num4 < 99)
                            {
                                Vector2 Velocity1 = Vector2.UnitX * 16f;
                                if (!(Collision.TileCollision(Position - Velocity1, Velocity1, player.width, player.height, false, false, (int)player.gravDir) != Velocity1))
                                {
                                    Vector2 Velocity2 = -Vector2.UnitX * 16f;
                                    if (!(Collision.TileCollision(Position - Velocity2, Velocity2, player.width, player.height, false, false, (int)player.gravDir) != Velocity2))
                                    {
                                        Vector2 Velocity3 = Vector2.UnitY * 16f;
                                        if (!(Collision.TileCollision(Position - Velocity3, Velocity3, player.width, player.height, false, false, (int)player.gravDir) != Velocity3))
                                        {
                                            Vector2 Velocity4 = -Vector2.UnitY * 16f;
                                            if (!(Collision.TileCollision(Position - Velocity4, Velocity4, player.width, player.height, false, false, (int)player.gravDir) != Velocity4))
                                            {
                                                canSpawn = true;
                                                int num5 = index2 + num4;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Position;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (shadowGauntlet && item.melee)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame);
            }
        }

        public override void FrameEffects()
        {
            // Prevent potential bug with shot projectile detection.
            EndShotDetection();

            // Hide players wings, etc. when mounted
            if (player.mount.Active)
            {
                int mount = player.mount.Type;
                if (mount == CandyCopter._ref.Type)
                {
                    // Supposed to make players legs disappear, but only makes them skin-colored.
                    player.legs = -1;
                    player.wings = -1;
                    player.back = -1;
                    player.shield = -1;
                    // player.handoff = -1;
                    // player.handon = -1;
                }
                else if (mount == Drakomire._ref.Type)
                {
                    player.wings = -1;
                }
            }
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (daybloomSet && dazzleStacks != 0)
            {
                a = 255 - .0001180555f * dazzleStacks;
                if (dazzleStacks >= 3600)
                {
                    a = 255 - .0001180555f * 3600;
                }
            }

            if (toxify)
            {
                if (Main.rand.Next(2) == 0)
                {
                    int dust = Dust.NewDust(player.position, player.width + 4, 30, 110, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    Main.playerDrawDust.Add(dust);
                }

                r *= 0f;
                g *= 1f;
                b *= 0f;
                fullBright = true;
            }

            if (virulence <= 0)
            {
                if (Main.rand.NextBool(2))
                {
                    for (int index1 = 0; index1 < 4; ++index1)
                    {
                        int dust = Dust.NewDust(player.position, player.width, 30, 167, player.velocity.X, player.velocity.Y, 167, default, Main.rand.NextFloat(.4f, 1.2f));
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 1.8f;
                        Main.dust[dust].velocity.Y -= 0.5f;
                        Main.playerDrawDust.Add(dust);
                    }
                }
            }

            if (BlueDust)
            {
                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(player.position, player.width + 4, 30, 206, player.velocity.X, player.velocity.Y, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    Main.playerDrawDust.Add(dust);
                }

                r *= 0f;
                g *= 1f;
                b *= 0f;
                fullBright = true;
            }

            if (HellGaze)
            {
                if (Main.rand.Next(4) == 0)
                {
                    int dust = Dust.NewDust(player.position, player.width + 26, 30, 6, player.velocity.X, player.velocity.Y, 100, default, 1f);
                    Main.playerDrawDust.Add(dust);
                }

                r *= 0f;
                g *= 1f;
                b *= 0f;
                fullBright = true;
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (camoCounter > 0)
            {
                float camo = 1 - .75f / CAMO_DELAY * camoCounter;
                drawInfo.upperArmorColor = Color.Multiply(drawInfo.upperArmorColor, camo);
                drawInfo.middleArmorColor = Color.Multiply(drawInfo.middleArmorColor, camo);
                drawInfo.lowerArmorColor = Color.Multiply(drawInfo.lowerArmorColor, camo);
                camo *= camo;
                drawInfo.hairColor = Color.Multiply(drawInfo.hairColor, camo);
                drawInfo.eyeWhiteColor = Color.Multiply(drawInfo.eyeWhiteColor, camo);
                drawInfo.eyeColor = Color.Multiply(drawInfo.eyeColor, camo);
                drawInfo.faceColor = Color.Multiply(drawInfo.faceColor, camo);
                drawInfo.bodyColor = Color.Multiply(drawInfo.bodyColor, camo);
                drawInfo.legColor = Color.Multiply(drawInfo.legColor, camo);
                drawInfo.shirtColor = Color.Multiply(drawInfo.shirtColor, camo);
                drawInfo.underShirtColor = Color.Multiply(drawInfo.underShirtColor, camo);
                drawInfo.pantsColor = Color.Multiply(drawInfo.pantsColor, camo);
                drawInfo.shoeColor = Color.Multiply(drawInfo.shoeColor, camo);
                drawInfo.headGlowMaskColor = Color.Multiply(drawInfo.headGlowMaskColor, camo);
                drawInfo.bodyGlowMaskColor = Color.Multiply(drawInfo.bodyGlowMaskColor, camo);
                drawInfo.armGlowMaskColor = Color.Multiply(drawInfo.armGlowMaskColor, camo);
                drawInfo.legGlowMaskColor = Color.Multiply(drawInfo.legGlowMaskColor, camo);
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if ((drakomireMount || basiliskMount) && layers[i].Name == "Wings")
                {
                    layers[i].visible = false;
                }

                if (layers[i].Name == "HeldItem")
                {
                    if (player.inventory[player.selectedItem].type == mod.ItemType("HexBow") && player.itemAnimation > 0)
                    {
                        weaponAnimationCounter++;
                        if (weaponAnimationCounter >= 10)
                        {
                            hexBowAnimationFrame = (hexBowAnimationFrame + 1) % 4;
                            weaponAnimationCounter = 0;
                        }
                        layers[i] = WeaponLayer;
                    }
                }
            }

            if (bubbleTimer > 0)
            {
                BubbleLayer.visible = true;
                layers.Add(BubbleLayer);
            }
        }

        public static readonly PlayerLayer WeaponLayer = new PlayerLayer("SpiritMod", "WeaponLayer", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player drawPlayer = drawInfo.drawPlayer;
            MyPlayer myPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && !drawPlayer.outOfRange)
            {
                Texture2D weaponTexture = Main.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type];
                Vector2 vector8 = new Vector2(weaponTexture.Width / 2, (weaponTexture.Height / 4) / 2);
                Vector2 vector9 = new Vector2(8, 0);

                vector8.Y = vector9.Y;

                Vector2 vector = drawPlayer.itemLocation;
                vector.Y += weaponTexture.Height * 0.5F;

                int num84 = (int)vector9.X;
                Vector2 origin2 = new Vector2(-num84, (weaponTexture.Height / 4) / 2);
                if (drawPlayer.direction == -1)
                {
                    origin2 = new Vector2(weaponTexture.Width + num84, (weaponTexture.Height / 4) / 2);
                }

                SpriteEffects effect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                DrawData drawData = new DrawData(weaponTexture, new Vector2((int)(vector.X - Main.screenPosition.X + vector8.X), (int)(vector.Y - Main.screenPosition.Y + vector8.Y)), new Rectangle?(new Rectangle(0, (weaponTexture.Height / 4) * myPlayer.hexBowAnimationFrame, weaponTexture.Width, weaponTexture.Height / 4)), drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(Color.White), drawPlayer.itemRotation, origin2, drawPlayer.inventory[drawPlayer.selectedItem].scale, effect, 0);
                Main.playerDrawData.Add(drawData);

                if (drawPlayer.inventory[drawPlayer.selectedItem].color != default)
                {
                    drawData = new DrawData(weaponTexture, new Vector2((int)(vector.X - Main.screenPosition.X + vector8.X), (int)(vector.Y - Main.screenPosition.Y + vector8.Y)), new Rectangle?(new Rectangle(0, (weaponTexture.Height / 4) * myPlayer.hexBowAnimationFrame, weaponTexture.Width, weaponTexture.Height / 4)), drawPlayer.inventory[drawPlayer.selectedItem].GetColor(Color.White), drawPlayer.itemRotation, origin2, drawPlayer.inventory[drawPlayer.selectedItem].scale, effect, 0);
                    Main.playerDrawData.Add(drawData);
                }
            }
        });

        public static readonly PlayerLayer BubbleLayer = new PlayerLayer("SpiritMod", "BubbleLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Mod mod = ModLoader.GetMod("SpiritMod");
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.active && !drawPlayer.outOfRange)
            {
                Texture2D texture = mod.GetTexture("Effects/PlayerVisuals/BubbleShield_Visual");
                Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

                Vector2 drawPos = drawPlayer.position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f);
                drawPos.X = (int)drawPos.X;
                drawPos.Y = (int)drawPos.Y;

                DrawData drawData = new DrawData(texture, drawPos - Main.screenPosition, new Rectangle?(), Color.White * 0.75f, 0, origin, 1, SpriteEffects.None, 0);
                Main.playerDrawData.Add(drawData);
            }
        });
    }
}