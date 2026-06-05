using System;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Provides extension methods and properties for <see cref="Script"/>
/// </summary>
public static class ScriptExtensions
{
	private static readonly FrozenDictionary<Script, (string code, string name, int number)> mData;
	private static readonly FrozenDictionary<int, Script> mNumberLookup;

	static ScriptExtensions()
	{
		// Please see the comment in the ModuleInitializer method for the reason why we're doing it this way instead of static field initializers.

		mData = FrozenDictionary.ToFrozenDictionary<Script, (string code, string name, int number)>([
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'d' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'m')), ("Adlm", "Adlam", 166)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'f' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'k')), ("Afak", "Afaka", 439)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'b')), ("Aghb", "Caucasian Albanian", 239)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'m')), ("Ahom", "Ahom, Tai Ahom", 338)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'b')), ("Arab", "Arabic", 160)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'n')), ("Aran", "Arabic (Nastaliq variant)", 161)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'i')), ("Armi", "Imperial Aramaic", 124)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'n')), ("Armn", "Armenian", 230)),
			new(unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'v' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'t')), ("Avst", "Avestan", 134)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'i')), ("Bali", "Balinese", 360)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'u')), ("Bamu", "Bamum", 435)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'s')), ("Bass", "Bassa Vah", 259)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'k')), ("Batk", "Batak", 365)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Beng", "Bengali (Bangla)", 325)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'f')), ("Berf", "Beria Erfe", 258)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'s')), ("Bhks", "Bhaiksuki", 334)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'s')), ("Blis", "Blissymbols", 550)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'o')), ("Bopo", "Bopomofo", 285)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'h')), ("Brah", "Brahmi", 300)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'i')), ("Brai", "Braille", 570)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'i')), ("Bugi", "Buginese", 367)),
			new(unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'d')), ("Buhd", "Buhid", 372)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'m')), ("Cakm", "Chakma", 349)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'s')), ("Cans", "Unified Canadian Aboriginal Syllabics", 440)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'i')), ("Cari", "Carian", 201)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'m')), ("Cham", "Cham", 358)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'r')), ("Cher", "Cherokee", 445)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'s')), ("Chis", "Chisoi", 298)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'s')), ("Chrs", "Chorasmian", 109)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'t')), ("Cirt", "Cirth", 291)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'t')), ("Copt", "Coptic", 204)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'n')), ("Cpmn", "Cypro-Minoan", 402)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'t')), ("Cprt", "Cypriot syllabary", 403)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'l')), ("Cyrl", "Cyrillic", 220)),
			new(unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'s')), ("Cyrs", "Cyrillic (Old Church Slavonic variant)", 221)),
			new(unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'v' << 8 | (uint)(byte)'a')), ("Deva", "Devanagari (Nagari)", 315)),
			new(unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'k')), ("Diak", "Dives Akuru", 342)),
			new(unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'r')), ("Dogr", "Dogra", 328)),
			new(unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'t')), ("Dsrt", "Deseret (Mormon)", 250)),
			new(unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'l')), ("Dupl", "Duployan shorthand, Duployan stenography", 755)),
			new(unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'d')), ("Egyd", "Egyptian demotic", 70)),
			new(unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'h')), ("Egyh", "Egyptian hieratic", 60)),
			new(unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'p')), ("Egyp", "Egyptian hieroglyphs", 50)),
			new(unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'a')), ("Elba", "Elbasan", 226)),
			new(unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'m')), ("Elym", "Elymaic", 128)),
			new(unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'i')), ("Ethi", "Ethiopic (Geʻez)", 430)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a')), ("Gara", "Garay", 164)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'k')), ("Geok", "Khutsuri (Asomtavruli and Nuskhuri)", 241)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'r')), ("Geor", "Georgian (Mkhedruli and Mtavruli)", 240)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'g')), ("Glag", "Glagolitic", 225)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Gong", "Gunjala Gondi", 312)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'m')), ("Gonm", "Masaram Gondi", 313)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'h')), ("Goth", "Gothic", 206)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'n')), ("Gran", "Grantha", 343)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'k')), ("Grek", "Greek", 200)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'j' << 8 | (uint)(byte)'r')), ("Gujr", "Gujarati", 320)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'h')), ("Gukh", "Gurung Khema", 397)),
			new(unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'u')), ("Guru", "Gurmukhi", 310)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'b')), ("Hanb", "Han with Bopomofo (alias for Han + Bopomofo)", 503)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Hang", "Hangul (Hangŭl, Hangeul)", 286)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'i')), ("Hani", "Han (Hanzi, Kanji, Hanja)", 500)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'o')), ("Hano", "Hanunoo (Hanunóo)", 371)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'s')), ("Hans", "Han (Simplified variant)", 501)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'t')), ("Hant", "Han (Traditional variant)", 502)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'r')), ("Hatr", "Hatran", 127)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'r')), ("Hebr", "Hebrew", 125)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a')), ("Hira", "Hiragana", 410)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'w')), ("Hluw", "Anatolian Hieroglyphs (Luwian Hieroglyphs, Hittite Hieroglyphs)", 80)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'m' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Hmng", "Pahawh Hmong", 450)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'m' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'p')), ("Hmnp", "Nyiakeng Puachue Hmong", 451)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'l')), ("Hntl", "Han (Traditional variant) with Latin (alias for Hant + Latn)", 504)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'t')), ("Hrkt", "Japanese syllabaries (alias for Hiragana + Katakana)", 412)),
			new(unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Hung", "Old Hungarian (Hungarian Runic)", 176)),
			new(unchecked((Script)((uint)(byte)'I' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'s')), ("Inds", "Indus (Harappan)", 610)),
			new(unchecked((Script)((uint)(byte)'I' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'l')), ("Ital", "Old Italic (Etruscan, Oscan, etc.)", 210)),
			new(unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'o')), ("Jamo", "Jamo (alias for Jamo subset of Hangul)", 284)),
			new(unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'v' << 8 | (uint)(byte)'a')), ("Java", "Javanese", 361)),
			new(unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'n')), ("Jpan", "Japanese (alias for Han + Hiragana + Katakana)", 413)),
			new(unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c')), ("Jurc", "Jurchen", 510)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'i')), ("Kali", "Kayah Li", 357)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'a')), ("Kana", "Katakana", 411)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'w' << 8 | (uint)(byte)'i')), ("Kawi", "Kawi", 368)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'r')), ("Khar", "Kharoshthi", 305)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'r')), ("Khmr", "Khmer", 355)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'j')), ("Khoj", "Khojki", 322)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'l')), ("Kitl", "Khitan large script", 505)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'s')), ("Kits", "Khitan small script", 288)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'a')), ("Knda", "Kannada", 345)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'e')), ("Kore", "Korean (alias for Hangul + Han)", 287)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'l')), ("Kpel", "Kpelle", 436)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'i')), ("Krai", "Kirat Rai", 396)),
			new(unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'i')), ("Kthi", "Kaithi", 317)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'a')), ("Lana", "Tai Tham (Lanna)", 351)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'o')), ("Laoo", "Lao", 356)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'f')), ("Latf", "Latin (Fraktur variant)", 217)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'g')), ("Latg", "Latin (Gaelic variant)", 216)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'n')), ("Latn", "Latin", 215)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'e')), ("Leke", "Leke", 364)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'c')), ("Lepc", "Lepcha (Róng)", 335)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'b')), ("Limb", "Limbu", 336)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'a')), ("Lina", "Linear A", 400)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'b')), ("Linb", "Linear B", 401)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'u')), ("Lisu", "Lisu (Fraser)", 399)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'a')), ("Loma", "Loma", 437)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'c' << 8 | (uint)(byte)'i')), ("Lyci", "Lycian", 202)),
			new(unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'i')), ("Lydi", "Lydian", 116)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'j')), ("Mahj", "Mahajani", 314)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'a')), ("Maka", "Makasar", 366)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d')), ("Mand", "Mandaic, Mandaean", 140)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'i')), ("Mani", "Manichaean", 139)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c')), ("Marc", "Marchen", 332)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'a')), ("Maya", "Mayan hieroglyphs", 90)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'f')), ("Medf", "Medefaidrin (Oberi Okaime, Oberi Ɔkaimɛ)", 265)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d')), ("Mend", "Mende Kikakui", 438)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c')), ("Merc", "Meroitic Cursive", 101)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'o')), ("Mero", "Meroitic Hieroglyphs", 100)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'m')), ("Mlym", "Malayalam", 347)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'i')), ("Modi", "Modi, Moḍī", 324)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Mong", "Mongolian", 145)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'n')), ("Moon", "Moon (Moon code, Moon script, Moon type)", 218)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'o')), ("Mroo", "Mro, Mru", 264)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'i')), ("Mtei", "Meitei Mayek (Meithei, Meetei)", 337)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'t')), ("Mult", "Multani", 323)),
			new(unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'r')), ("Mymr", "Myanmar (Burmese)", 350)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'m')), ("Nagm", "Nag Mundari", 295)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d')), ("Nand", "Nandinagari", 311)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'b')), ("Narb", "Old North Arabian (Ancient North Arabian)", 106)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'b' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'t')), ("Nbat", "Nabataean", 159)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'w' << 8 | (uint)(byte)'a')), ("Newa", "Newa, Newar, Newari, Nepāla lipi", 333)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'k' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'b')), ("Nkdb", "Naxi Dongba (na²¹ɕi³³ to³³ba²¹, Nakhi Tomba)", 85)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'k' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'b')), ("Nkgb", "Naxi Geba (na²¹ɕi³³ gʌ²¹ba²¹, 'Na-'Khi ²Ggŏ-¹baw, Nakhi Geba)", 420)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'k' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'o')), ("Nkoo", "N’Ko", 165)),
			new(unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'u')), ("Nshu", "Nüshu", 499)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'m')), ("Ogam", "Ogham", 212)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'c' << 8 | (uint)(byte)'k')), ("Olck", "Ol Chiki (Ol Cemet’, Ol, Santali)", 261)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'o')), ("Onao", "Ol Onal", 296)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'h')), ("Orkh", "Old Turkic, Orkhon Runic", 175)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'a')), ("Orya", "Oriya (Odia)", 327)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'e')), ("Osge", "Osage", 219)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'a')), ("Osma", "Osmanya", 260)),
			new(unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'r')), ("Ougr", "Old Uyghur", 143)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'m')), ("Palm", "Palmyrene", 126)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'c')), ("Pauc", "Pau Cin Hau", 263)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'c' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'n')), ("Pcun", "Proto-Cuneiform", 15)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'m')), ("Pelm", "Proto-Elamite", 16)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'m')), ("Perm", "Old Permic", 227)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'g')), ("Phag", "Phags-pa", 331)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'i')), ("Phli", "Inscriptional Pahlavi", 131)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'p')), ("Phlp", "Psalter Pahlavi", 132)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'v')), ("Phlv", "Book Pahlavi", 133)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'x')), ("Phnx", "Phoenician", 115)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'q' << 8 | (uint)(byte)'d')), ("Piqd", "Klingon (KLI pIqaD)", 293)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'d')), ("Plrd", "Miao (Pollard)", 282)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'i')), ("Prti", "Inscriptional Parthian", 130)),
			new(unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'n')), ("Psin", "Proto-Sinaitic", 103)),
			new(unchecked((Script)((uint)(byte)'Q' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'a')), ("Qaaa", "Reserved for private use (start)", 900)),
			new(unchecked((Script)((uint)(byte)'Q' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'x')), ("Qabx", "Reserved for private use (end)", 949)),
			new(unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'j')), ("Ranj", "Ranjana", 303)),
			new(unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'j' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Rjng", "Rejang (Redjang, Kaganga)", 363)),
			new(unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'g')), ("Rohg", "Hanifi Rohingya", 167)),
			new(unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'o')), ("Roro", "Rongorongo", 620)),
			new(unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'r')), ("Runr", "Runic", 211)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'r')), ("Samr", "Samaritan", 123)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a')), ("Sara", "Sarati", 292)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'b')), ("Sarb", "Old South Arabian", 105)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'r')), ("Saur", "Saurashtra", 344)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'l')), ("Seal", "(Small) Seal", 590)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'w')), ("Sgnw", "SignWriting", 95)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'w')), ("Shaw", "Shavian (Shaw)", 281)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'d')), ("Shrd", "Sharada, Śāradā", 319)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'i')), ("Shui", "Shuishu", 530)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'d')), ("Sidd", "Siddham, Siddhaṃ, Siddhamātṛkā", 302)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'t')), ("Sidt", "Sidetic", 180)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d')), ("Sind", "Khudawadi, Sindhi", 318)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'h')), ("Sinh", "Sinhala", 348)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'d')), ("Sogd", "Sogdian", 141)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'o')), ("Sogo", "Old Sogdian", 142)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a')), ("Sora", "Sora Sompeng", 398)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'o')), ("Soyo", "Soyombo", 329)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d')), ("Sund", "Sundanese", 362)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'u')), ("Sunu", "Sunuwar", 274)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'o')), ("Sylo", "Syloti Nagri", 316)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c')), ("Syrc", "Syriac", 135)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'e')), ("Syre", "Syriac (Estrangelo variant)", 138)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'j')), ("Syrj", "Syriac (Western variant)", 137)),
			new(unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'n')), ("Syrn", "Syriac (Eastern variant)", 136)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'b')), ("Tagb", "Tagbanwa", 373)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'r')), ("Takr", "Takri, Ṭākrī, Ṭāṅkrī", 321)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'e')), ("Tale", "Tai Le", 353)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'u')), ("Talu", "New Tai Lue", 354)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'l')), ("Taml", "Tamil", 346)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Tang", "Tangut", 520)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'v' << 8 | (uint)(byte)'t')), ("Tavt", "Tai Viet", 359)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'o')), ("Tayo", "Tai Yo", 380)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'u')), ("Telu", "Telugu", 340)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Teng", "Tengwar", 290)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'f' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g')), ("Tfng", "Tifinagh (Berber)", 120)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'g')), ("Tglg", "Tagalog (Baybayin, Alibata)", 370)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'a')), ("Thaa", "Thaana", 170)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'i')), ("Thai", "Thai", 352)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'t')), ("Tibt", "Tibetan", 330)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'h')), ("Tirh", "Tirhuta", 326)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'a')), ("Tnsa", "Tangsa", 275)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'r')), ("Todr", "Todhri", 229)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'s')), ("Tols", "Tolong Siki", 299)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'o')), ("Toto", "Toto", 294)),
			new(unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'g')), ("Tutg", "Tulu-Tigalari", 341)),
			new(unchecked((Script)((uint)(byte)'U' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'r')), ("Ugar", "Ugaritic", 40)),
			new(unchecked((Script)((uint)(byte)'V' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'i')), ("Vaii", "Vai", 470)),
			new(unchecked((Script)((uint)(byte)'V' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'p')), ("Visp", "Visible Speech", 280)),
			new(unchecked((Script)((uint)(byte)'V' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'h')), ("Vith", "Vithkuqi", 228)),
			new(unchecked((Script)((uint)(byte)'W' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a')), ("Wara", "Warang Citi (Varang Kshiti)", 262)),
			new(unchecked((Script)((uint)(byte)'W' << 24 | (uint)(byte)'c' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'o')), ("Wcho", "Wancho", 283)),
			new(unchecked((Script)((uint)(byte)'W' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'e')), ("Wole", "Woleai", 480)),
			new(unchecked((Script)((uint)(byte)'X' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'o')), ("Xpeo", "Old Persian", 30)),
			new(unchecked((Script)((uint)(byte)'X' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'x')), ("Xsux", "Cuneiform, Sumero-Akkadian", 20)),
			new(unchecked((Script)((uint)(byte)'Y' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'z' << 8 | (uint)(byte)'i')), ("Yezi", "Yezidi", 192)),
			new(unchecked((Script)((uint)(byte)'Y' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'i')), ("Yiii", "Yi", 460)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'b')), ("Zanb", "Zanabazar Square (Zanabazarin Dörböljin Useg, Xewtee Dörböljin Bicig, Horizontal Square Script)", 339)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'h')), ("Zinh", "Code for inherited script", 994)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'m' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'h')), ("Zmth", "Mathematical notation", 995)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'e')), ("Zsye", "Symbols (Emoji variant)", 993)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'m')), ("Zsym", "Symbols", 996)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'x' << 16 | (uint)(byte)'x' << 8 | (uint)(byte)'x')), ("Zxxx", "Code for unwritten documents", 997)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'y')), ("Zyyy", "Code for undetermined script", 998)),
			new(unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'z' << 16 | (uint)(byte)'z' << 8 | (uint)(byte)'z')), ("Zzzz", "Code for uncoded script", 999)),
		]);

		mNumberLookup = FrozenDictionary.ToFrozenDictionary<int, Script>([
			new(166, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'d' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'m'))),
			new(439, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'f' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'k'))),
			new(239, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'b'))),
			new(338, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'m'))),
			new(160, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'b'))),
			new(161, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'n'))),
			new(124, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'i'))),
			new(230, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'n'))),
			new(134, unchecked((Script)((uint)(byte)'A' << 24 | (uint)(byte)'v' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'t'))),
			new(360, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'i'))),
			new(435, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'u'))),
			new(259, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'s'))),
			new(365, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'k'))),
			new(325, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(258, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'f'))),
			new(334, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'s'))),
			new(550, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'s'))),
			new(285, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'o'))),
			new(300, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'h'))),
			new(570, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'i'))),
			new(367, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'i'))),
			new(372, unchecked((Script)((uint)(byte)'B' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'d'))),
			new(349, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'m'))),
			new(440, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'s'))),
			new(201, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'i'))),
			new(358, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'m'))),
			new(445, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'r'))),
			new(298, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'s'))),
			new(109, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'s'))),
			new(291, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'t'))),
			new(204, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'t'))),
			new(402, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'n'))),
			new(403, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'t'))),
			new(220, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'l'))),
			new(221, unchecked((Script)((uint)(byte)'C' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'s'))),
			new(315, unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'v' << 8 | (uint)(byte)'a'))),
			new(342, unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'k'))),
			new(328, unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'r'))),
			new(250, unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'t'))),
			new(755, unchecked((Script)((uint)(byte)'D' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'l'))),
			new(70, unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'d'))),
			new(60, unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'h'))),
			new(50, unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'p'))),
			new(226, unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'a'))),
			new(128, unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'m'))),
			new(430, unchecked((Script)((uint)(byte)'E' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'i'))),
			new(164, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a'))),
			new(241, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'k'))),
			new(240, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'r'))),
			new(225, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'g'))),
			new(312, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(313, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'m'))),
			new(206, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'h'))),
			new(343, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'n'))),
			new(200, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'k'))),
			new(320, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'j' << 8 | (uint)(byte)'r'))),
			new(397, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'h'))),
			new(310, unchecked((Script)((uint)(byte)'G' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'u'))),
			new(503, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'b'))),
			new(286, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(500, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'i'))),
			new(371, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'o'))),
			new(501, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'s'))),
			new(502, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'t'))),
			new(127, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'r'))),
			new(125, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'r'))),
			new(410, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a'))),
			new(80, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'w'))),
			new(450, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'m' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(451, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'m' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'p'))),
			new(504, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'l'))),
			new(412, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'t'))),
			new(176, unchecked((Script)((uint)(byte)'H' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(610, unchecked((Script)((uint)(byte)'I' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'s'))),
			new(210, unchecked((Script)((uint)(byte)'I' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'l'))),
			new(284, unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'o'))),
			new(361, unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'v' << 8 | (uint)(byte)'a'))),
			new(413, unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'n'))),
			new(510, unchecked((Script)((uint)(byte)'J' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c'))),
			new(357, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'i'))),
			new(411, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'a'))),
			new(368, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'w' << 8 | (uint)(byte)'i'))),
			new(305, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'r'))),
			new(355, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'r'))),
			new(322, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'j'))),
			new(505, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'l'))),
			new(288, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'s'))),
			new(345, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'a'))),
			new(287, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'e'))),
			new(436, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'l'))),
			new(396, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'i'))),
			new(317, unchecked((Script)((uint)(byte)'K' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'i'))),
			new(351, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'a'))),
			new(356, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'o'))),
			new(217, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'f'))),
			new(216, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'g'))),
			new(215, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'n'))),
			new(364, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'e'))),
			new(335, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'p' << 8 | (uint)(byte)'c'))),
			new(336, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'b'))),
			new(400, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'a'))),
			new(401, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'b'))),
			new(399, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'u'))),
			new(437, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'a'))),
			new(202, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'c' << 8 | (uint)(byte)'i'))),
			new(116, unchecked((Script)((uint)(byte)'L' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'i'))),
			new(314, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'j'))),
			new(366, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'a'))),
			new(140, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d'))),
			new(139, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'i'))),
			new(332, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c'))),
			new(90, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'a'))),
			new(265, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'f'))),
			new(438, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d'))),
			new(101, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c'))),
			new(100, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'o'))),
			new(347, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'m'))),
			new(324, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'i'))),
			new(145, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(218, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'n'))),
			new(264, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'o'))),
			new(337, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'t' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'i'))),
			new(323, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'t'))),
			new(350, unchecked((Script)((uint)(byte)'M' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'r'))),
			new(295, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'m'))),
			new(311, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d'))),
			new(106, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'b'))),
			new(159, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'b' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'t'))),
			new(333, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'w' << 8 | (uint)(byte)'a'))),
			new(85, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'k' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'b'))),
			new(420, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'k' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'b'))),
			new(165, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'k' << 16 | (uint)(byte)'o' << 8 | (uint)(byte)'o'))),
			new(499, unchecked((Script)((uint)(byte)'N' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'u'))),
			new(212, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'m'))),
			new(261, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'c' << 8 | (uint)(byte)'k'))),
			new(296, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'o'))),
			new(175, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'h'))),
			new(327, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'a'))),
			new(219, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'e'))),
			new(260, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'a'))),
			new(143, unchecked((Script)((uint)(byte)'O' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'r'))),
			new(126, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'m'))),
			new(263, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'c'))),
			new(15, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'c' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'n'))),
			new(16, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'m'))),
			new(227, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'m'))),
			new(331, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'g'))),
			new(131, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'i'))),
			new(132, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'p'))),
			new(133, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'v'))),
			new(115, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'x'))),
			new(293, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'q' << 8 | (uint)(byte)'d'))),
			new(282, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'l' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'d'))),
			new(130, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'r' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'i'))),
			new(103, unchecked((Script)((uint)(byte)'P' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'n'))),
			new(900, unchecked((Script)((uint)(byte)'Q' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'a'))),
			new(949, unchecked((Script)((uint)(byte)'Q' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'x'))),
			new(303, unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'j'))),
			new(363, unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'j' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(167, unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'g'))),
			new(620, unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'o'))),
			new(211, unchecked((Script)((uint)(byte)'R' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'r'))),
			new(123, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'r'))),
			new(292, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a'))),
			new(105, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'b'))),
			new(344, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'r'))),
			new(590, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'l'))),
			new(95, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'w'))),
			new(281, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'w'))),
			new(319, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'d'))),
			new(530, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'i'))),
			new(302, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'d'))),
			new(180, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'t'))),
			new(318, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d'))),
			new(348, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'h'))),
			new(141, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'d'))),
			new(142, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'o'))),
			new(398, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a'))),
			new(329, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'o'))),
			new(362, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'d'))),
			new(274, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'u'))),
			new(316, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'o'))),
			new(135, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'c'))),
			new(138, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'e'))),
			new(137, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'j'))),
			new(136, unchecked((Script)((uint)(byte)'S' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'n'))),
			new(373, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'g' << 8 | (uint)(byte)'b'))),
			new(321, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'k' << 8 | (uint)(byte)'r'))),
			new(353, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'e'))),
			new(354, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'u'))),
			new(346, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'m' << 8 | (uint)(byte)'l'))),
			new(520, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(359, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'v' << 8 | (uint)(byte)'t'))),
			new(380, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'o'))),
			new(340, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'u'))),
			new(290, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(120, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'f' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'g'))),
			new(370, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'g'))),
			new(170, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'a'))),
			new(352, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'h' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'i'))),
			new(330, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'b' << 8 | (uint)(byte)'t'))),
			new(326, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'h'))),
			new(275, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'n' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'a'))),
			new(229, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'d' << 8 | (uint)(byte)'r'))),
			new(299, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'s'))),
			new(294, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'o'))),
			new(341, unchecked((Script)((uint)(byte)'T' << 24 | (uint)(byte)'u' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'g'))),
			new(40, unchecked((Script)((uint)(byte)'U' << 24 | (uint)(byte)'g' << 16 | (uint)(byte)'a' << 8 | (uint)(byte)'r'))),
			new(470, unchecked((Script)((uint)(byte)'V' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'i'))),
			new(280, unchecked((Script)((uint)(byte)'V' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'s' << 8 | (uint)(byte)'p'))),
			new(228, unchecked((Script)((uint)(byte)'V' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'h'))),
			new(262, unchecked((Script)((uint)(byte)'W' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'r' << 8 | (uint)(byte)'a'))),
			new(283, unchecked((Script)((uint)(byte)'W' << 24 | (uint)(byte)'c' << 16 | (uint)(byte)'h' << 8 | (uint)(byte)'o'))),
			new(480, unchecked((Script)((uint)(byte)'W' << 24 | (uint)(byte)'o' << 16 | (uint)(byte)'l' << 8 | (uint)(byte)'e'))),
			new(30, unchecked((Script)((uint)(byte)'X' << 24 | (uint)(byte)'p' << 16 | (uint)(byte)'e' << 8 | (uint)(byte)'o'))),
			new(20, unchecked((Script)((uint)(byte)'X' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'u' << 8 | (uint)(byte)'x'))),
			new(192, unchecked((Script)((uint)(byte)'Y' << 24 | (uint)(byte)'e' << 16 | (uint)(byte)'z' << 8 | (uint)(byte)'i'))),
			new(460, unchecked((Script)((uint)(byte)'Y' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'i' << 8 | (uint)(byte)'i'))),
			new(339, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'a' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'b'))),
			new(994, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'i' << 16 | (uint)(byte)'n' << 8 | (uint)(byte)'h'))),
			new(995, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'m' << 16 | (uint)(byte)'t' << 8 | (uint)(byte)'h'))),
			new(993, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'e'))),
			new(996, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'s' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'m'))),
			new(997, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'x' << 16 | (uint)(byte)'x' << 8 | (uint)(byte)'x'))),
			new(998, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'y' << 16 | (uint)(byte)'y' << 8 | (uint)(byte)'y'))),
			new(999, unchecked((Script)((uint)(byte)'Z' << 24 | (uint)(byte)'z' << 16 | (uint)(byte)'z' << 8 | (uint)(byte)'z'))),
		]);
	}

#pragma warning disable CA2255 // I'm sorry, but we're doing this now...
	[ModuleInitializer]
#pragma warning restore CA2255
	internal static void ModuleInitializer()
	{
		// This forces the frozen dictionaries to be initialized at module load time (via the static type constructor).
		// Otherwise, if we would just rely on static initialization, the first access to an extension property or method would trigger the frozen dictionaries to get initialized.
		//
		// Initializing a frozen dictionary is a very expensive operation, but comes with the benefit of accessing an initialized frozen dictionary being very fast!
		//
		// That's why we want to move initialization time into module load time, so we don't risk the first access to, for example, ScriptExtensions.Code to be very slow.
		// With that, accessing ScriptExtensions.Code, ScriptExtensions.Name, or ScriptExtensions.TryFromCode, etc., will be fast at all times, even the first time,
		// thanks to the frozen dictionaries being already initialized and frozen dictionaries being very optimized for fast access.

		RuntimeHelpers.RunClassConstructor(typeof(ScriptExtensions).TypeHandle);
	}

	/// <exception cref="InvalidOperationException">The <see cref="Script"/> is not a recognized as a known script</exception>
	[DoesNotReturn]
	private static void FailUnrecognizedScript(Script script) => throw new InvalidOperationException($"Unrecognized script: {script}");

	extension(Script)
	{
		/// <summary>
		/// Tries to get the <see cref="Script"/> corresponding to the given 4-character <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see> code
		/// </summary>
		/// <param name="code">The case-insensitive 4-character <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see> code to get the corresponding <see cref="Script"/> for</param>
		/// <param name="script">The <see cref="Script"/> corresponding to the given 4-character <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see> code, if this method returns <c><see langword="true"/></c>; otherwise, the <c><see langword="default"/>(<see cref="Script"/>)</c></param>
		/// <returns><c><see langword="true"/></c>, if the given <paramref name="code"/> was recognized to be a known <see cref="Script"/>; otherwise, <c><see langword="false"/></c></returns>
		public static bool TryFromCode(string code, out Script script)
		{
			if (code is not [var c0, var c1, var c2, var c3]
			|| !char.IsAsciiLetter(c0)
			|| !char.IsAsciiLetter(c1)
			|| !char.IsAsciiLetter(c2)
			|| !char.IsAsciiLetter(c3))
			{
				script = default;

				return false;
			}

			script = unchecked((Script)((uint)(byte)char.ToUpperInvariant(c0) << 24 | (uint)(byte)char.ToUpperInvariant(c1) << 16 | (uint)(byte)char.ToUpperInvariant(c2) << 8 | (uint)(byte)char.ToUpperInvariant(c3)));

			return true;
		}

		/// <summary>
		/// Tries to get the <see cref="Script"/> corresponding to the given number assigned to a script as defined by <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see>
		/// </summary>
		/// <param name="number">The number assigned to a script as defined by <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see></param>
		/// <param name="script">The <see cref="Script"/> corresponding to the given number, if this method returns <c><see langword="true"/></c>; otherwise, the <c><see langword="default"/>(<see cref="Script"/>)</c></param>
		/// <returns><c><see langword="true"/></c>, if the given <paramref name="number"/> was recognized to be a known <see cref="Script"/>; otherwise, <c><see langword="false"/></c></returns>
		public static bool TryFromNumber(int number, out Script script)
		{
			if (!mNumberLookup.TryGetValue(number, out script))
			{
				script = default;

				return false;
			}

			return true;
		}
	}

	extension(Script script)
	{
		/// <summary>
		/// Gets the 4-character <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see> code for the <see cref="Script"/>
		/// </summary>
		/// <value>
		/// The 4-character <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see> code for the <see cref="Script"/>
		/// </value>
		public string Code
		{
			get
			{
				if (!mData.TryGetValue(script, out var data))
				{
					var numericScript = unchecked((uint)script);

					Span<char> code = stackalloc char[4];
					code[3] = unchecked((char)(byte)numericScript);
					code[2] = unchecked((char)(byte)(numericScript >>= 8));
					code[1] = unchecked((char)(byte)(numericScript >>= 8));
					code[0] = unchecked((char)(byte)(numericScript >>= 8));

					return code.ToString();
				}

				return data.code;
			}
		}

		/// <summary>
		/// Gets the number assigned to the <see cref="Script"/> as defined by <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see>
		/// </summary>
		/// <value>
		/// The number assigned to the <see cref="Script"/> as defined by <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see>
		/// </value>
		/// <inheritdoc cref="FailUnrecognizedScript(Script)"/>
		public int Number
		{
			get
			{
				if (!mData.TryGetValue(script, out var data))
				{
					FailUnrecognizedScript(script);
				}

				return data.number;
			}
		}

		/// <summary>
		/// Gets the natural, English name of the <see cref="Script"/> as defined by <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see>
		/// </summary>
		/// <value>
		/// The natural, English name of the <see cref="Script"/> as defined by <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924</see>
		/// </value>
		/// <inheritdoc cref="FailUnrecognizedScript(Script)"/>
		public string Name
		{
			get
			{
				if (!mData.TryGetValue(script, out var data))
				{
					FailUnrecognizedScript(script);
				}

				return data.name;
			}
		}
	}
}
