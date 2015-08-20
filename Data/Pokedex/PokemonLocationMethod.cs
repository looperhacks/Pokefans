﻿// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public class PokemonLocationMethod
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
