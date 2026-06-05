namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents an <see href="https://unicode.org/iso15924/iso15924-codes.html">ISO 15924 script</see> 
/// </summary>
public enum Script : uint
{
	/// <summary>Adlam</summary>
	Adlam = (byte)'A' << 24 | (byte)'d' << 16 | (byte)'l' << 8 | (byte)'m',

	/// <summary>Afaka</summary>
	Afaka = (byte)'A' << 24 | (byte)'f' << 16 | (byte)'a' << 8 | (byte)'k',

	/// <summary>Caucasian Albanian</summary>
	CaucasianAlbanian = (byte)'A' << 24 | (byte)'g' << 16 | (byte)'h' << 8 | (byte)'b',

	/// <summary>Ahom, Tai Ahom</summary>
	Ahom = (byte)'A' << 24 | (byte)'h' << 16 | (byte)'o' << 8 | (byte)'m',

	/// <summary>Arabic</summary>
	Arabic = (byte)'A' << 24 | (byte)'r' << 16 | (byte)'a' << 8 | (byte)'b',

	/// <summary>Arabic (Nastaliq variant)</summary>
	ArabicNastaliq = (byte)'A' << 24 | (byte)'r' << 16 | (byte)'a' << 8 | (byte)'n',

	/// <summary>Imperial Aramaic</summary>
	ImperialAramaic = (byte)'A' << 24 | (byte)'r' << 16 | (byte)'m' << 8 | (byte)'i',

	/// <summary>Armenian</summary>
	Armenian = (byte)'A' << 24 | (byte)'r' << 16 | (byte)'m' << 8 | (byte)'n',

	/// <summary>Avestan</summary>
	Avestan = (byte)'A' << 24 | (byte)'v' << 16 | (byte)'s' << 8 | (byte)'t',

	/// <summary>Balinese</summary>
	Balinese = (byte)'B' << 24 | (byte)'a' << 16 | (byte)'l' << 8 | (byte)'i',

	/// <summary>Bamum</summary>
	Bamum = (byte)'B' << 24 | (byte)'a' << 16 | (byte)'m' << 8 | (byte)'u',

	/// <summary>Bassa Vah</summary>
	BassaVah = (byte)'B' << 24 | (byte)'a' << 16 | (byte)'s' << 8 | (byte)'s',

	/// <summary>Batak</summary>
	Batak = (byte)'B' << 24 | (byte)'a' << 16 | (byte)'t' << 8 | (byte)'k',

	/// <summary>Bengali (Bangla)</summary>
	Bengali = (byte)'B' << 24 | (byte)'e' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Beria Erfe</summary>
	BeriaErfe = (byte)'B' << 24 | (byte)'e' << 16 | (byte)'r' << 8 | (byte)'f',

	/// <summary>Bhaiksuki</summary>
	Bhaiksuki = (byte)'B' << 24 | (byte)'h' << 16 | (byte)'k' << 8 | (byte)'s',

	/// <summary>Blissymbols</summary>
	Blissymbols = (byte)'B' << 24 | (byte)'l' << 16 | (byte)'i' << 8 | (byte)'s',

	/// <summary>Bopomofo</summary>
	Bopomofo = (byte)'B' << 24 | (byte)'o' << 16 | (byte)'p' << 8 | (byte)'o',

	/// <summary>Brahmi</summary>
	Brahmi = (byte)'B' << 24 | (byte)'r' << 16 | (byte)'a' << 8 | (byte)'h',

	/// <summary>Braille</summary>
	Braille = (byte)'B' << 24 | (byte)'r' << 16 | (byte)'a' << 8 | (byte)'i',

	/// <summary>Buginese</summary>
	Buginese = (byte)'B' << 24 | (byte)'u' << 16 | (byte)'g' << 8 | (byte)'i',

	/// <summary>Buhid</summary>
	Buhid = (byte)'B' << 24 | (byte)'u' << 16 | (byte)'h' << 8 | (byte)'d',

	/// <summary>Chakma</summary>
	Chakma = (byte)'C' << 24 | (byte)'a' << 16 | (byte)'k' << 8 | (byte)'m',

	/// <summary>Unified Canadian Aboriginal Syllabics</summary>
	CanadianAboriginal = (byte)'C' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'s',

	/// <summary>Carian</summary>
	Carian = (byte)'C' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'i',

	/// <summary>Cham</summary>
	Cham = (byte)'C' << 24 | (byte)'h' << 16 | (byte)'a' << 8 | (byte)'m',

	/// <summary>Cherokee</summary>
	Cherokee = (byte)'C' << 24 | (byte)'h' << 16 | (byte)'e' << 8 | (byte)'r',

	/// <summary>Chisoi</summary>
	Chisoi = (byte)'C' << 24 | (byte)'h' << 16 | (byte)'i' << 8 | (byte)'s',

	/// <summary>Chorasmian</summary>
	Chorasmian = (byte)'C' << 24 | (byte)'h' << 16 | (byte)'r' << 8 | (byte)'s',

	/// <summary>Cirth</summary>
	Cirth = (byte)'C' << 24 | (byte)'i' << 16 | (byte)'r' << 8 | (byte)'t',

	/// <summary>Coptic</summary>
	Coptic = (byte)'C' << 24 | (byte)'o' << 16 | (byte)'p' << 8 | (byte)'t',

	/// <summary>Cypro-Minoan</summary>
	CyproMinoan = (byte)'C' << 24 | (byte)'p' << 16 | (byte)'m' << 8 | (byte)'n',

	/// <summary>Cypriot syllabary</summary>
	Cypriot = (byte)'C' << 24 | (byte)'p' << 16 | (byte)'r' << 8 | (byte)'t',

	/// <summary>Cyrillic</summary>
	Cyrillic = (byte)'C' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'l',

	/// <summary>Cyrillic (Old Church Slavonic variant)</summary>
	CyrillicOldChurchSlavonic = (byte)'C' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'s',

	/// <summary>Devanagari (Nagari)</summary>
	Devanagari = (byte)'D' << 24 | (byte)'e' << 16 | (byte)'v' << 8 | (byte)'a',

	/// <summary>Dives Akuru</summary>
	DivesAkuru = (byte)'D' << 24 | (byte)'i' << 16 | (byte)'a' << 8 | (byte)'k',

	/// <summary>Dogra</summary>
	Dogra = (byte)'D' << 24 | (byte)'o' << 16 | (byte)'g' << 8 | (byte)'r',

	/// <summary>Deseret (Mormon)</summary>
	Deseret = (byte)'D' << 24 | (byte)'s' << 16 | (byte)'r' << 8 | (byte)'t',

	/// <summary>Duployan shorthand, Duployan stenography</summary>
	Duployan = (byte)'D' << 24 | (byte)'u' << 16 | (byte)'p' << 8 | (byte)'l',

	/// <summary>Egyptian demotic</summary>
	EgyptianDemotic = (byte)'E' << 24 | (byte)'g' << 16 | (byte)'y' << 8 | (byte)'d',

	/// <summary>Egyptian hieratic</summary>
	EgyptianHieratic = (byte)'E' << 24 | (byte)'g' << 16 | (byte)'y' << 8 | (byte)'h',

	/// <summary>Egyptian hieroglyphs</summary>
	EgyptianHieroglyphs = (byte)'E' << 24 | (byte)'g' << 16 | (byte)'y' << 8 | (byte)'p',

	/// <summary>Elbasan</summary>
	Elbasan = (byte)'E' << 24 | (byte)'l' << 16 | (byte)'b' << 8 | (byte)'a',

	/// <summary>Elymaic</summary>
	Elymaic = (byte)'E' << 24 | (byte)'l' << 16 | (byte)'y' << 8 | (byte)'m',

	/// <summary>Ethiopic (Geʻez)</summary>
	Ethiopic = (byte)'E' << 24 | (byte)'t' << 16 | (byte)'h' << 8 | (byte)'i',

	/// <summary>Garay</summary>
	Garay = (byte)'G' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'a',

	/// <summary>Khutsuri (Asomtavruli and Nuskhuri)</summary>
	Khutsuri = (byte)'G' << 24 | (byte)'e' << 16 | (byte)'o' << 8 | (byte)'k',

	/// <summary>Georgian (Mkhedruli and Mtavruli)</summary>
	Georgian = (byte)'G' << 24 | (byte)'e' << 16 | (byte)'o' << 8 | (byte)'r',

	/// <summary>Glagolitic</summary>
	Glagolitic = (byte)'G' << 24 | (byte)'l' << 16 | (byte)'a' << 8 | (byte)'g',

	/// <summary>Gunjala Gondi</summary>
	GunjalaGondi = (byte)'G' << 24 | (byte)'o' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Masaram Gondi</summary>
	MasaramGondi = (byte)'G' << 24 | (byte)'o' << 16 | (byte)'n' << 8 | (byte)'m',

	/// <summary>Gothic</summary>
	Gothic = (byte)'G' << 24 | (byte)'o' << 16 | (byte)'t' << 8 | (byte)'h',

	/// <summary>Grantha</summary>
	Grantha = (byte)'G' << 24 | (byte)'r' << 16 | (byte)'a' << 8 | (byte)'n',

	/// <summary>Greek</summary>
	Greek = (byte)'G' << 24 | (byte)'r' << 16 | (byte)'e' << 8 | (byte)'k',

	/// <summary>Gujarati</summary>
	Gujarati = (byte)'G' << 24 | (byte)'u' << 16 | (byte)'j' << 8 | (byte)'r',

	/// <summary>Gurung Khema</summary>
	GurungKhema = (byte)'G' << 24 | (byte)'u' << 16 | (byte)'k' << 8 | (byte)'h',

	/// <summary>Gurmukhi</summary>
	Gurmukhi = (byte)'G' << 24 | (byte)'u' << 16 | (byte)'r' << 8 | (byte)'u',

	/// <summary>Han with Bopomofo (alias for Han + Bopomofo)</summary>
	HanWithBopomofo = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'b',

	/// <summary>Hangul (Hangŭl, Hangeul)</summary>
	Hangul = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Han (Hanzi, Kanji, Hanja)</summary>
	Han = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'i',

	/// <summary>Hanunoo (Hanunóo)</summary>
	Hanunoo = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'o',

	/// <summary>Han (Simplified variant)</summary>
	HanSimplified = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'s',

	/// <summary>Han (Traditional variant)</summary>
	HanTraditional = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'t',

	/// <summary>Hatran</summary>
	Hatran = (byte)'H' << 24 | (byte)'a' << 16 | (byte)'t' << 8 | (byte)'r',

	/// <summary>Hebrew</summary>
	Hebrew = (byte)'H' << 24 | (byte)'e' << 16 | (byte)'b' << 8 | (byte)'r',

	/// <summary>Hiragana</summary>
	Hiragana = (byte)'H' << 24 | (byte)'i' << 16 | (byte)'r' << 8 | (byte)'a',

	/// <summary>Anatolian Hieroglyphs (Luwian Hieroglyphs, Hittite Hieroglyphs)</summary>
	AnatolianHieroglyphs = (byte)'H' << 24 | (byte)'l' << 16 | (byte)'u' << 8 | (byte)'w',

	/// <summary>Pahawh Hmong</summary>
	PahawhHmong = (byte)'H' << 24 | (byte)'m' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Nyiakeng Puachue Hmong</summary>
	NyiakengPuachueHmong = (byte)'H' << 24 | (byte)'m' << 16 | (byte)'n' << 8 | (byte)'p',

	/// <summary>Han (Traditional variant) with Latin (alias for Hant + Latn)</summary>
	HanTraditionalWithLatin = (byte)'H' << 24 | (byte)'n' << 16 | (byte)'t' << 8 | (byte)'l',

	/// <summary>Japanese syllabaries (alias for Hiragana + Katakana)</summary>
	KatakanaOrHiragana = (byte)'H' << 24 | (byte)'r' << 16 | (byte)'k' << 8 | (byte)'t',

	/// <summary>Old Hungarian (Hungarian Runic)</summary>
	OldHungarian = (byte)'H' << 24 | (byte)'u' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Indus (Harappan)</summary>
	IndusHarappan = (byte)'I' << 24 | (byte)'n' << 16 | (byte)'d' << 8 | (byte)'s',

	/// <summary>Old Italic (Etruscan, Oscan, etc.)</summary>
	OldItalic = (byte)'I' << 24 | (byte)'t' << 16 | (byte)'a' << 8 | (byte)'l',

	/// <summary>Jamo (alias for Jamo subset of Hangul)</summary>
	Jamo = (byte)'J' << 24 | (byte)'a' << 16 | (byte)'m' << 8 | (byte)'o',

	/// <summary>Javanese</summary>
	Javanese = (byte)'J' << 24 | (byte)'a' << 16 | (byte)'v' << 8 | (byte)'a',

	/// <summary>Japanese (alias for Han + Hiragana + Katakana)</summary>
	Japanese = (byte)'J' << 24 | (byte)'p' << 16 | (byte)'a' << 8 | (byte)'n',

	/// <summary>Jurchen</summary>
	Jurchen = (byte)'J' << 24 | (byte)'u' << 16 | (byte)'r' << 8 | (byte)'c',

	/// <summary>Kayah Li</summary>
	KayahLi = (byte)'K' << 24 | (byte)'a' << 16 | (byte)'l' << 8 | (byte)'i',

	/// <summary>Katakana</summary>
	Katakana = (byte)'K' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'a',

	/// <summary>Kawi</summary>
	Kawi = (byte)'K' << 24 | (byte)'a' << 16 | (byte)'w' << 8 | (byte)'i',

	/// <summary>Kharoshthi</summary>
	Kharoshthi = (byte)'K' << 24 | (byte)'h' << 16 | (byte)'a' << 8 | (byte)'r',

	/// <summary>Khmer</summary>
	Khmer = (byte)'K' << 24 | (byte)'h' << 16 | (byte)'m' << 8 | (byte)'r',

	/// <summary>Khojki</summary>
	Khojki = (byte)'K' << 24 | (byte)'h' << 16 | (byte)'o' << 8 | (byte)'j',

	/// <summary>Khitan large script</summary>
	KhitanLarge = (byte)'K' << 24 | (byte)'i' << 16 | (byte)'t' << 8 | (byte)'l',

	/// <summary>Khitan small script</summary>
	KhitanSmall = (byte)'K' << 24 | (byte)'i' << 16 | (byte)'t' << 8 | (byte)'s',

	/// <summary>Kannada</summary>
	Kannada = (byte)'K' << 24 | (byte)'n' << 16 | (byte)'d' << 8 | (byte)'a',

	/// <summary>Korean (alias for Hangul + Han)</summary>
	Korean = (byte)'K' << 24 | (byte)'o' << 16 | (byte)'r' << 8 | (byte)'e',

	/// <summary>Kpelle</summary>
	Kpelle = (byte)'K' << 24 | (byte)'p' << 16 | (byte)'e' << 8 | (byte)'l',

	/// <summary>Kirat Rai</summary>
	KiratRai = (byte)'K' << 24 | (byte)'r' << 16 | (byte)'a' << 8 | (byte)'i',

	/// <summary>Kaithi</summary>
	Kaithi = (byte)'K' << 24 | (byte)'t' << 16 | (byte)'h' << 8 | (byte)'i',

	/// <summary>Tai Tham (Lanna)</summary>
	TaiTham = (byte)'L' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'a',

	/// <summary>Lao</summary>
	Lao = (byte)'L' << 24 | (byte)'a' << 16 | (byte)'o' << 8 | (byte)'o',

	/// <summary>Latin (Fraktur variant)</summary>
	LatinFraktur = (byte)'L' << 24 | (byte)'a' << 16 | (byte)'t' << 8 | (byte)'f',

	/// <summary>Latin (Gaelic variant)</summary>
	LatinGaelic = (byte)'L' << 24 | (byte)'a' << 16 | (byte)'t' << 8 | (byte)'g',

	/// <summary>Latin</summary>
	Latin = (byte)'L' << 24 | (byte)'a' << 16 | (byte)'t' << 8 | (byte)'n',

	/// <summary>Leke</summary>
	Leke = (byte)'L' << 24 | (byte)'e' << 16 | (byte)'k' << 8 | (byte)'e',

	/// <summary>Lepcha (Róng)</summary>
	Lepcha = (byte)'L' << 24 | (byte)'e' << 16 | (byte)'p' << 8 | (byte)'c',

	/// <summary>Limbu</summary>
	Limbu = (byte)'L' << 24 | (byte)'i' << 16 | (byte)'m' << 8 | (byte)'b',

	/// <summary>Linear A</summary>
	LinearA = (byte)'L' << 24 | (byte)'i' << 16 | (byte)'n' << 8 | (byte)'a',

	/// <summary>Linear B</summary>
	LinearB = (byte)'L' << 24 | (byte)'i' << 16 | (byte)'n' << 8 | (byte)'b',

	/// <summary>Lisu (Fraser)</summary>
	Lisu = (byte)'L' << 24 | (byte)'i' << 16 | (byte)'s' << 8 | (byte)'u',

	/// <summary>Loma</summary>
	Loma = (byte)'L' << 24 | (byte)'o' << 16 | (byte)'m' << 8 | (byte)'a',

	/// <summary>Lycian</summary>
	Lycian = (byte)'L' << 24 | (byte)'y' << 16 | (byte)'c' << 8 | (byte)'i',

	/// <summary>Lydian</summary>
	Lydian = (byte)'L' << 24 | (byte)'y' << 16 | (byte)'d' << 8 | (byte)'i',

	/// <summary>Mahajani</summary>
	Mahajani = (byte)'M' << 24 | (byte)'a' << 16 | (byte)'h' << 8 | (byte)'j',

	/// <summary>Makasar</summary>
	Makasar = (byte)'M' << 24 | (byte)'a' << 16 | (byte)'k' << 8 | (byte)'a',

	/// <summary>Mandaic, Mandaean</summary>
	Mandaic = (byte)'M' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'d',

	/// <summary>Manichaean</summary>
	Manichaean = (byte)'M' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'i',

	/// <summary>Marchen</summary>
	Marchen = (byte)'M' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'c',

	/// <summary>Mayan hieroglyphs</summary>
	MayanHieroglyphs = (byte)'M' << 24 | (byte)'a' << 16 | (byte)'y' << 8 | (byte)'a',

	/// <summary>Medefaidrin (Oberi Okaime, Oberi Ɔkaimɛ)</summary>
	Medefaidrin = (byte)'M' << 24 | (byte)'e' << 16 | (byte)'d' << 8 | (byte)'f',

	/// <summary>Mende Kikakui</summary>
	MendeKikakui = (byte)'M' << 24 | (byte)'e' << 16 | (byte)'n' << 8 | (byte)'d',

	/// <summary>Meroitic Cursive</summary>
	MeroiticCursive = (byte)'M' << 24 | (byte)'e' << 16 | (byte)'r' << 8 | (byte)'c',

	/// <summary>Meroitic Hieroglyphs</summary>
	MeroiticHieroglyphs = (byte)'M' << 24 | (byte)'e' << 16 | (byte)'r' << 8 | (byte)'o',

	/// <summary>Malayalam</summary>
	Malayalam = (byte)'M' << 24 | (byte)'l' << 16 | (byte)'y' << 8 | (byte)'m',

	/// <summary>Modi, Moḍī</summary>
	Modi = (byte)'M' << 24 | (byte)'o' << 16 | (byte)'d' << 8 | (byte)'i',

	/// <summary>Mongolian</summary>
	Mongolian = (byte)'M' << 24 | (byte)'o' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Moon (Moon code, Moon script, Moon type)</summary>
	Moon = (byte)'M' << 24 | (byte)'o' << 16 | (byte)'o' << 8 | (byte)'n',

	/// <summary>Mro, Mru</summary>
	Mro = (byte)'M' << 24 | (byte)'r' << 16 | (byte)'o' << 8 | (byte)'o',

	/// <summary>Meitei Mayek (Meithei, Meetei)</summary>
	MeeteiMayek = (byte)'M' << 24 | (byte)'t' << 16 | (byte)'e' << 8 | (byte)'i',

	/// <summary>Multani</summary>
	Multani = (byte)'M' << 24 | (byte)'u' << 16 | (byte)'l' << 8 | (byte)'t',

	/// <summary>Myanmar (Burmese)</summary>
	Myanmar = (byte)'M' << 24 | (byte)'y' << 16 | (byte)'m' << 8 | (byte)'r',

	/// <summary>Nag Mundari</summary>
	NagMundari = (byte)'N' << 24 | (byte)'a' << 16 | (byte)'g' << 8 | (byte)'m',

	/// <summary>Nandinagari</summary>
	Nandinagari = (byte)'N' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'d',

	/// <summary>Old North Arabian (Ancient North Arabian)</summary>
	OldNorthArabian = (byte)'N' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'b',

	/// <summary>Nabataean</summary>
	Nabataean = (byte)'N' << 24 | (byte)'b' << 16 | (byte)'a' << 8 | (byte)'t',

	/// <summary>Newa, Newar, Newari, Nepāla lipi</summary>
	Newa = (byte)'N' << 24 | (byte)'e' << 16 | (byte)'w' << 8 | (byte)'a',

	/// <summary>Naxi Dongba (na²¹ɕi³³ to³³ba²¹, Nakhi Tomba)</summary>
	NaxiDongba = (byte)'N' << 24 | (byte)'k' << 16 | (byte)'d' << 8 | (byte)'b',

	/// <summary>Naxi Geba (na²¹ɕi³³ gʌ²¹ba²¹, 'Na-'Khi ²Ggŏ-¹baw, Nakhi Geba)</summary>
	NaxiGeba = (byte)'N' << 24 | (byte)'k' << 16 | (byte)'g' << 8 | (byte)'b',

	/// <summary>N’Ko</summary>
	Nko = (byte)'N' << 24 | (byte)'k' << 16 | (byte)'o' << 8 | (byte)'o',

	/// <summary>Nüshu</summary>
	Nushu = (byte)'N' << 24 | (byte)'s' << 16 | (byte)'h' << 8 | (byte)'u',

	/// <summary>Ogham</summary>
	Ogham = (byte)'O' << 24 | (byte)'g' << 16 | (byte)'a' << 8 | (byte)'m',

	/// <summary>Ol Chiki (Ol Cemet’, Ol, Santali)</summary>
	OlChiki = (byte)'O' << 24 | (byte)'l' << 16 | (byte)'c' << 8 | (byte)'k',

	/// <summary>Ol Onal</summary>
	OlOnal = (byte)'O' << 24 | (byte)'n' << 16 | (byte)'a' << 8 | (byte)'o',

	/// <summary>Old Turkic, Orkhon Runic</summary>
	OldTurkic = (byte)'O' << 24 | (byte)'r' << 16 | (byte)'k' << 8 | (byte)'h',

	/// <summary>Oriya (Odia)</summary>
	Oriya = (byte)'O' << 24 | (byte)'r' << 16 | (byte)'y' << 8 | (byte)'a',

	/// <summary>Osage</summary>
	Osage = (byte)'O' << 24 | (byte)'s' << 16 | (byte)'g' << 8 | (byte)'e',

	/// <summary>Osmanya</summary>
	Osmanya = (byte)'O' << 24 | (byte)'s' << 16 | (byte)'m' << 8 | (byte)'a',

	/// <summary>Old Uyghur</summary>
	OldUyghur = (byte)'O' << 24 | (byte)'u' << 16 | (byte)'g' << 8 | (byte)'r',

	/// <summary>Palmyrene</summary>
	Palmyrene = (byte)'P' << 24 | (byte)'a' << 16 | (byte)'l' << 8 | (byte)'m',

	/// <summary>Pau Cin Hau</summary>
	PauCinHau = (byte)'P' << 24 | (byte)'a' << 16 | (byte)'u' << 8 | (byte)'c',

	/// <summary>Proto-Cuneiform</summary>
	ProtoCuneiform = (byte)'P' << 24 | (byte)'c' << 16 | (byte)'u' << 8 | (byte)'n',

	/// <summary>Proto-Elamite</summary>
	ProtoElamite = (byte)'P' << 24 | (byte)'e' << 16 | (byte)'l' << 8 | (byte)'m',

	/// <summary>Old Permic</summary>
	OldPermic = (byte)'P' << 24 | (byte)'e' << 16 | (byte)'r' << 8 | (byte)'m',

	/// <summary>Phags-pa</summary>
	PhagsPa = (byte)'P' << 24 | (byte)'h' << 16 | (byte)'a' << 8 | (byte)'g',

	/// <summary>Inscriptional Pahlavi</summary>
	InscriptionalPahlavi = (byte)'P' << 24 | (byte)'h' << 16 | (byte)'l' << 8 | (byte)'i',

	/// <summary>Psalter Pahlavi</summary>
	PsalterPahlavi = (byte)'P' << 24 | (byte)'h' << 16 | (byte)'l' << 8 | (byte)'p',

	/// <summary>Book Pahlavi</summary>
	BookPahlavi = (byte)'P' << 24 | (byte)'h' << 16 | (byte)'l' << 8 | (byte)'v',

	/// <summary>Phoenician</summary>
	Phoenician = (byte)'P' << 24 | (byte)'h' << 16 | (byte)'n' << 8 | (byte)'x',

	/// <summary>Klingon (KLI pIqaD)</summary>
	Klingon = (byte)'P' << 24 | (byte)'i' << 16 | (byte)'q' << 8 | (byte)'d',

	/// <summary>Miao (Pollard)</summary>
	Miao = (byte)'P' << 24 | (byte)'l' << 16 | (byte)'r' << 8 | (byte)'d',

	/// <summary>Inscriptional Parthian</summary>
	InscriptionalParthian = (byte)'P' << 24 | (byte)'r' << 16 | (byte)'t' << 8 | (byte)'i',

	/// <summary>Proto-Sinaitic</summary>
	ProtoSinaitic = (byte)'P' << 24 | (byte)'s' << 16 | (byte)'i' << 8 | (byte)'n',

    /// <summary>Ranjana</summary>
    Ranjana = (byte)'R' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'j',

    /// <summary>Rejang (Redjang, Kaganga)</summary>
    Rejang = (byte)'R' << 24 | (byte)'j' << 16 | (byte)'n' << 8 | (byte)'g',

    /// <summary>Hanifi Rohingya</summary>
    HanifiRohingya = (byte)'R' << 24 | (byte)'o' << 16 | (byte)'h' << 8 | (byte)'g',

    /// <summary>Rongorongo</summary>
    Rongorongo = (byte)'R' << 24 | (byte)'o' << 16 | (byte)'r' << 8 | (byte)'o',

    /// <summary>Runic</summary>
    Runic = (byte)'R' << 24 | (byte)'u' << 16 | (byte)'n' << 8 | (byte)'r',

    /// <summary>Samaritan</summary>
    Samaritan = (byte)'S' << 24 | (byte)'a' << 16 | (byte)'m' << 8 | (byte)'r',

    /// <summary>Sarati</summary>
    Sarati = (byte)'S' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'a',

    /// <summary>Old South Arabian</summary>
    OldSouthArabian = (byte)'S' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'b',

    /// <summary>Saurashtra</summary>
    Saurashtra = (byte)'S' << 24 | (byte)'a' << 16 | (byte)'u' << 8 | (byte)'r',

    /// <summary>(Small) Seal</summary>
    Seal = (byte)'S' << 24 | (byte)'e' << 16 | (byte)'a' << 8 | (byte)'l',

	/// <summary>SignWriting</summary>
	SignWriting = (byte)'S' << 24 | (byte)'g' << 16 | (byte)'n' << 8 | (byte)'w',

	/// <summary>Shavian (Shaw)</summary>
	Shavian = (byte)'S' << 24 | (byte)'h' << 16 | (byte)'a' << 8 | (byte)'w',

	/// <summary>Sharada, Śāradā</summary>
	Sharada = (byte)'S' << 24 | (byte)'h' << 16 | (byte)'r' << 8 | (byte)'d',

	/// <summary>Shuishu</summary>
	Shuishu = (byte)'S' << 24 | (byte)'h' << 16 | (byte)'u' << 8 | (byte)'i',

	/// <summary>Siddham, Siddhaṃ, Siddhamātṛkā</summary>
	Siddham = (byte)'S' << 24 | (byte)'i' << 16 | (byte)'d' << 8 | (byte)'d',

	/// <summary>Sidetic</summary>
	Sidetic = (byte)'S' << 24 | (byte)'i' << 16 | (byte)'d' << 8 | (byte)'t',

	/// <summary>Khudawadi, Sindhi</summary>
	Khudawadi = (byte)'S' << 24 | (byte)'i' << 16 | (byte)'n' << 8 | (byte)'d',

	/// <summary>Sinhala</summary>
	Sinhala = (byte)'S' << 24 | (byte)'i' << 16 | (byte)'n' << 8 | (byte)'h',

	/// <summary>Sogdian</summary>
	Sogdian = (byte)'S' << 24 | (byte)'o' << 16 | (byte)'g' << 8 | (byte)'d',

	/// <summary>Old Sogdian</summary>
	OldSogdian = (byte)'S' << 24 | (byte)'o' << 16 | (byte)'g' << 8 | (byte)'o',

	/// <summary>Sora Sompeng</summary>
	SoraSompeng = (byte)'S' << 24 | (byte)'o' << 16 | (byte)'r' << 8 | (byte)'a',

	/// <summary>Soyombo</summary>
	Soyombo = (byte)'S' << 24 | (byte)'o' << 16 | (byte)'y' << 8 | (byte)'o',

	/// <summary>Sundanese</summary>
	Sundanese = (byte)'S' << 24 | (byte)'u' << 16 | (byte)'n' << 8 | (byte)'d',

	/// <summary>Sunuwar</summary>
	Sunuwar = (byte)'S' << 24 | (byte)'u' << 16 | (byte)'n' << 8 | (byte)'u',

	/// <summary>Syloti Nagri</summary>
	SylotiNagri = (byte)'S' << 24 | (byte)'y' << 16 | (byte)'l' << 8 | (byte)'o',

	/// <summary>Syriac</summary>
	Syriac = (byte)'S' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'c',

	/// <summary>Syriac (Estrangelo variant)</summary>
	SyriacEstrangelo = (byte)'S' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'e',

    /// <summary>Syriac (Western variant)</summary>
    SyriacWestern = (byte)'S' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'j',

    /// <summary>Syriac (Eastern variant)</summary>
    SyriacEastern = (byte)'S' << 24 | (byte)'y' << 16 | (byte)'r' << 8 | (byte)'n',

    /// <summary>Tagbanwa</summary>
    Tagbanwa = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'g' << 8 | (byte)'b',

    /// <summary>Takri, Ṭākrī, Ṭāṅkrī</summary>
    Takri = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'k' << 8 | (byte)'r',

    /// <summary>Tai Le</summary>
    TaiLe = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'l' << 8 | (byte)'e',

    /// <summary>New Tai Lue</summary>
    NewTaiLue = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'l' << 8 | (byte)'u',

    /// <summary>Tamil</summary>
    Tamil = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'m' << 8 | (byte)'l',

    /// <summary>Tangut</summary>
    Tangut = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'g',

    /// <summary>Tai Viet</summary>
    TaiViet = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'v' << 8 | (byte)'t',

    /// <summary>Tai Yo</summary>
    TaiYo = (byte)'T' << 24 | (byte)'a' << 16 | (byte)'y' << 8 | (byte)'o',

	/// <summary>Telugu</summary>
	Telugu = (byte)'T' << 24 | (byte)'e' << 16 | (byte)'l' << 8 | (byte)'u',

	/// <summary>Tengwar</summary>
	Tengwar = (byte)'T' << 24 | (byte)'e' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Tifinagh (Berber)</summary>
	Tifinagh = (byte)'T' << 24 | (byte)'f' << 16 | (byte)'n' << 8 | (byte)'g',

	/// <summary>Tagalog (Baybayin, Alibata)</summary>
	Tagalog = (byte)'T' << 24 | (byte)'g' << 16 | (byte)'l' << 8 | (byte)'g',

	/// <summary>Thaana</summary>
	Thaana = (byte)'T' << 24 | (byte)'h' << 16 | (byte)'a' << 8 | (byte)'a',

	/// <summary>Thai</summary>
	Thai = (byte)'T' << 24 | (byte)'h' << 16 | (byte)'a' << 8 | (byte)'i',

	/// <summary>Tibetan</summary>
	Tibetan = (byte)'T' << 24 | (byte)'i' << 16 | (byte)'b' << 8 | (byte)'t',

	/// <summary>Tirhuta</summary>
	Tirhuta = (byte)'T' << 24 | (byte)'i' << 16 | (byte)'r' << 8 | (byte)'h',

	/// <summary>Tangsa</summary>
	Tangsa = (byte)'T' << 24 | (byte)'n' << 16 | (byte)'s' << 8 | (byte)'a',

	/// <summary>Todhri</summary>
	Todhri = (byte)'T' << 24 | (byte)'o' << 16 | (byte)'d' << 8 | (byte)'r',

	/// <summary>Tolong Siki</summary>
	TolongSiki = (byte)'T' << 24 | (byte)'o' << 16 | (byte)'l' << 8 | (byte)'s',

    /// <summary>Toto</summary>
    Toto = (byte)'T' << 24 | (byte)'o' << 16 | (byte)'t' << 8 | (byte)'o',

    /// <summary>Tulu-Tigalari</summary>
    TuluTigalari = (byte)'T' << 24 | (byte)'u' << 16 | (byte)'t' << 8 | (byte)'g',

    /// <summary>Ugaritic</summary>
    Ugaritic = (byte)'U' << 24 | (byte)'g' << 16 | (byte)'a' << 8 | (byte)'r',

    /// <summary>Vai</summary>
    Vai = (byte)'V' << 24 | (byte)'a' << 16 | (byte)'i' << 8 | (byte)'i',

    /// <summary>Visible Speech</summary>
    VisibleSpeech = (byte)'V' << 24 | (byte)'i' << 16 | (byte)'s' << 8 | (byte)'p',

	/// <summary>Vithkuqi</summary>
	Vithkuqi = (byte)'V' << 24 | (byte)'i' << 16 | (byte)'t' << 8 | (byte)'h',

	/// <summary>Warang Citi (Varang Kshiti)</summary>
	WarangCiti = (byte)'W' << 24 | (byte)'a' << 16 | (byte)'r' << 8 | (byte)'a',

	/// <summary>Wancho</summary>
	Wancho = (byte)'W' << 24 | (byte)'c' << 16 | (byte)'h' << 8 | (byte)'o',

	/// <summary>Woleai</summary>
	Woleai = (byte)'W' << 24 | (byte)'o' << 16 | (byte)'l' << 8 | (byte)'e',

	/// <summary>Old Persian</summary>
	OldPersian = (byte)'X' << 24 | (byte)'p' << 16 | (byte)'e' << 8 | (byte)'o',

	/// <summary>Cuneiform, Sumero-Akkadian</summary>
	Cuneiform = (byte)'X' << 24 | (byte)'s' << 16 | (byte)'u' << 8 | (byte)'x',

	/// <summary>Yezidi</summary>
	Yezidi = (byte)'Y' << 24 | (byte)'e' << 16 | (byte)'z' << 8 | (byte)'i',

	/// <summary>Yi</summary>
	Yi = (byte)'Y' << 24 | (byte)'i' << 16 | (byte)'i' << 8 | (byte)'i',

	/// <summary>Zanabazar Square (Zanabazarin Dörböljin Useg, Xewtee Dörböljin Bicig, Horizontal Square Script)</summary>
	ZanabazarSquare = (byte)'Z' << 24 | (byte)'a' << 16 | (byte)'n' << 8 | (byte)'b',

	/// <summary>Code for inherited script</summary>
	Inherited = (byte)'Z' << 24 | (byte)'i' << 16 | (byte)'n' << 8 | (byte)'h',

	/// <summary>Mathematical notation</summary>
	MathematicalNotation = (byte)'Z' << 24 | (byte)'m' << 16 | (byte)'t' << 8 | (byte)'h',

    /// <summary>Symbols (Emoji variant)</summary>
    SymbolsEmoji = (byte)'Z' << 24 | (byte)'s' << 16 | (byte)'y' << 8 | (byte)'e',

    /// <summary>Symbols</summary>
    Symbols = (byte)'Z' << 24 | (byte)'s' << 16 | (byte)'y' << 8 | (byte)'m',

    /// <summary>Code for unwritten documents</summary>
    UnwrittenDocuments = (byte)'Z' << 24 | (byte)'x' << 16 | (byte)'x' << 8 | (byte)'x',

	/// <summary>Code for undetermined script</summary>
	Common = (byte)'Z' << 24 | (byte)'y' << 16 | (byte)'y' << 8 | (byte)'y',

	/// <summary>Code for uncoded script</summary>
	Unknown = (byte)'Z' << 24 | (byte)'z' << 16 | (byte)'z' << 8 | (byte)'z',
}
