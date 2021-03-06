﻿using System;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ppedv.TransportVisu.Model;

namespace ppedv.TransportVisu.Data.EF.Tests
{
    [TestClass]
    public class EfContextTest
    {
        [TestMethod]
        public void EfContext_can_create_DB()
        {
            using (var con = new EfContext())
            {
                if (con.Database.Exists())
                    con.Database.Delete();

                con.Database.Create();

                Assert.IsTrue(con.Database.Exists());
            }
        }

        [TestMethod]
        public void EfContext_can_CRUD_Waschmaschine()
        {
            var wm = new Waschmaschine()
            {
                Bezeichnung = "Schrubbeimer",
                Hersteller = "GL",
                Waschmittel = $"Aral {Guid.NewGuid()}"
            };

            var newWasch = $"Bersil {Guid.NewGuid()}";

            //CREATE
            using (var con = new EfContext())
            {
                con.Waschmaschinen.Add(wm);
                con.SaveChanges();
            }

            //READ
            using (var con = new EfContext())
            {
                var loaded = con.Waschmaschinen.Find(wm.Id);
                Assert.IsNotNull(loaded);
                Assert.AreEqual(wm.Waschmittel, loaded.Waschmittel);
                //UPDATE
                loaded.Waschmittel = newWasch;
                con.SaveChanges();
            }

            //Check UPDATE
            using (var con = new EfContext())
            {
                var loaded = con.Waschmaschinen.Find(wm.Id);
                Assert.AreEqual(newWasch, loaded.Waschmittel);
                //DELETE
                con.Waschmaschinen.Remove(loaded);
                con.SaveChanges();
            }

            //Check DELETE
            using (var con = new EfContext())
            {
                var loaded = con.Waschmaschinen.Find(wm.Id);
                Assert.IsNull(loaded);
            }
        }


        [TestMethod]
        public void EfContext_Wäsche()
        {
            var fix = new Fixture();
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            fix.Customizations.Add(new TypeRelay(typeof(Datenplatz), typeof(Puffer)));
            var waesche = fix.Create<Waesche>();

            using (var con = new EfContext())
            {
                con.Waesche.Add(waesche);
                con.SaveChanges();
            }

            using (var con = new EfContext())
            {
                var loaded = con.Waesche.Find(waesche.Id);
                loaded.Should().NotBeNull();

                loaded.Should().BeEquivalentTo(waesche, c => c.IgnoringCyclicReferences());
            }
        }


    }
}
