﻿using System;
using System.Linq;
using NUnit.Framework;

namespace SolrNet.Tests {
	[TestFixture]
	public class MappingManagerTests {
		[Test]
		public void AddAndGet() {
			var mgr = new MappingManager();
			mgr.Add(typeof (Entity).GetProperty("Id"), "id");
			var fields = mgr.GetFields(typeof (Entity));
			Assert.AreEqual(1, fields.Count);
		}

		[Test]
		public void No_Mapped_type_returns_null() {
			var mgr = new MappingManager();
			var fields = mgr.GetFields(typeof (Entity));
			Assert.IsNull(fields);
		}

		[Test]
		public void Add_duplicate_property_overwrites() {
			var mgr = new MappingManager();
			mgr.Add(typeof (Entity).GetProperty("Id"), "id");
			mgr.Add(typeof (Entity).GetProperty("Id"), "id2");
			var fields = mgr.GetFields(typeof (Entity));
			Assert.AreEqual(1, fields.Count);
			Assert.AreEqual("id2", fields.First().Value);
		}

		[Test]
		public void UniqueKey_Set_and_get() {
			var mgr = new MappingManager();
			var property = typeof (Entity).GetProperty("Id");
			mgr.Add(property, "id");
			mgr.SetUniqueKey(property);
			var key = mgr.GetUniqueKey(typeof (Entity));
			Assert.AreEqual(property, key.Key);
			Assert.AreEqual("id", key.Value);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Set_unique_key_without_mapping_throws() {
			var mgr = new MappingManager();
			var property = typeof(Entity).GetProperty("Id");
			mgr.SetUniqueKey(property);			
		}
	}

	public class Entity {
		public int Id { get; set; }
	}
}