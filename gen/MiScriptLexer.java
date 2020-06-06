// Generated from C:/Sources/Miki/MiScript/src/MiScript\MiScriptLexer.g4 by ANTLR 4.8
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class MiScriptLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.8", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		WhiteSpaces=1, Dot=2, If=3, End=4, Then=5, Else=6, Var=7, Is=8, Equal=9, 
		Do=10, NotEqual=11, Set=12, BeginString=13, IdentifierStart=14, Identifier=15, 
		EndExpr=16, EscapeInline=17, InlineExpr=18, EndString=19, Text=20, StartExpr=21, 
		Other=22;
	public static final int
		STRING_MODE=1, STRING_EXPR=2;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE", "STRING_MODE", "STRING_EXPR"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"WhiteSpaces", "Dot", "If", "End", "Then", "Else", "Var", "Is", "Equal", 
			"Do", "NotEqual", "Set", "BeginString", "IdentifierStart", "Identifier", 
			"EndExpr", "EscapeInline", "InlineExpr", "EndString", "Text", "StrIdentifier", 
			"StrDot", "StartExpr", "Other"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, "'if'", "'end'", "'then'", "'else'", "'var'", "'is'", 
			"'='", "'do'", "'!='", "'set'", null, null, null, "']'", "'$$'", null, 
			null, null, "'['"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "WhiteSpaces", "Dot", "If", "End", "Then", "Else", "Var", "Is", 
			"Equal", "Do", "NotEqual", "Set", "BeginString", "IdentifierStart", "Identifier", 
			"EndExpr", "EscapeInline", "InlineExpr", "EndString", "Text", "StartExpr", 
			"Other"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public MiScriptLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "MiScriptLexer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	@Override
	public void action(RuleContext _localctx, int ruleIndex, int actionIndex) {
		switch (ruleIndex) {
		case 23:
			Other_action((RuleContext)_localctx, actionIndex);
			break;
		}
	}
	private void Other_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 0:
			break;
		}
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2\30\u0095\b\1\b\1"+
		"\b\1\4\2\t\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4"+
		"\n\t\n\4\13\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t"+
		"\21\4\22\t\22\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t"+
		"\30\4\31\t\31\3\2\6\2\67\n\2\r\2\16\28\3\2\3\2\3\3\3\3\3\4\3\4\3\4\3\5"+
		"\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\6\3\7\3\7\3\7\3\7\3\7\3\b\3\b\3\b\3\b\3"+
		"\t\3\t\3\t\3\n\3\n\3\13\3\13\3\13\3\f\3\f\3\f\3\r\3\r\3\r\3\r\3\16\3\16"+
		"\3\16\3\16\3\17\3\17\3\20\6\20j\n\20\r\20\16\20k\3\21\3\21\3\21\3\21\3"+
		"\22\3\22\3\22\3\23\3\23\3\23\3\23\3\24\3\24\3\24\3\24\3\25\6\25~\n\25"+
		"\r\25\16\25\177\3\26\6\26\u0083\n\26\r\26\16\26\u0084\3\26\3\26\3\27\3"+
		"\27\3\27\3\27\3\30\3\30\3\30\3\30\3\31\3\31\3\31\3\31\3\31\2\2\32\5\3"+
		"\7\4\t\5\13\6\r\7\17\b\21\t\23\n\25\13\27\f\31\r\33\16\35\17\37\20!\21"+
		"#\22%\23\'\24)\25+\26-\2/\2\61\27\63\30\5\2\3\4\5\6\2\13\13\r\16\"\"\u00a2"+
		"\u00a2\6\2C\\^^aac|\4\2$$&&\2\u0096\2\5\3\2\2\2\2\7\3\2\2\2\2\t\3\2\2"+
		"\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\2\21\3\2\2\2\2\23\3\2\2\2\2\25"+
		"\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2\2\2\33\3\2\2\2\2\35\3\2\2\2\2\37\3\2"+
		"\2\2\2!\3\2\2\2\2#\3\2\2\2\3%\3\2\2\2\3\'\3\2\2\2\3)\3\2\2\2\3+\3\2\2"+
		"\2\4-\3\2\2\2\4/\3\2\2\2\4\61\3\2\2\2\4\63\3\2\2\2\5\66\3\2\2\2\7<\3\2"+
		"\2\2\t>\3\2\2\2\13A\3\2\2\2\rE\3\2\2\2\17J\3\2\2\2\21O\3\2\2\2\23S\3\2"+
		"\2\2\25V\3\2\2\2\27X\3\2\2\2\31[\3\2\2\2\33^\3\2\2\2\35b\3\2\2\2\37f\3"+
		"\2\2\2!i\3\2\2\2#m\3\2\2\2%q\3\2\2\2\'t\3\2\2\2)x\3\2\2\2+}\3\2\2\2-\u0082"+
		"\3\2\2\2/\u0088\3\2\2\2\61\u008c\3\2\2\2\63\u0090\3\2\2\2\65\67\t\2\2"+
		"\2\66\65\3\2\2\2\678\3\2\2\28\66\3\2\2\289\3\2\2\29:\3\2\2\2:;\b\2\2\2"+
		";\6\3\2\2\2<=\7\60\2\2=\b\3\2\2\2>?\7k\2\2?@\7h\2\2@\n\3\2\2\2AB\7g\2"+
		"\2BC\7p\2\2CD\7f\2\2D\f\3\2\2\2EF\7v\2\2FG\7j\2\2GH\7g\2\2HI\7p\2\2I\16"+
		"\3\2\2\2JK\7g\2\2KL\7n\2\2LM\7u\2\2MN\7g\2\2N\20\3\2\2\2OP\7x\2\2PQ\7"+
		"c\2\2QR\7t\2\2R\22\3\2\2\2ST\7k\2\2TU\7u\2\2U\24\3\2\2\2VW\7?\2\2W\26"+
		"\3\2\2\2XY\7f\2\2YZ\7q\2\2Z\30\3\2\2\2[\\\7#\2\2\\]\7?\2\2]\32\3\2\2\2"+
		"^_\7u\2\2_`\7g\2\2`a\7v\2\2a\34\3\2\2\2bc\7$\2\2cd\3\2\2\2de\b\16\3\2"+
		"e\36\3\2\2\2fg\7&\2\2g \3\2\2\2hj\t\3\2\2ih\3\2\2\2jk\3\2\2\2ki\3\2\2"+
		"\2kl\3\2\2\2l\"\3\2\2\2mn\7_\2\2no\3\2\2\2op\b\21\4\2p$\3\2\2\2qr\7&\2"+
		"\2rs\7&\2\2s&\3\2\2\2tu\7&\2\2uv\3\2\2\2vw\b\23\5\2w(\3\2\2\2xy\7$\2\2"+
		"yz\3\2\2\2z{\b\24\4\2{*\3\2\2\2|~\n\4\2\2}|\3\2\2\2~\177\3\2\2\2\177}"+
		"\3\2\2\2\177\u0080\3\2\2\2\u0080,\3\2\2\2\u0081\u0083\t\3\2\2\u0082\u0081"+
		"\3\2\2\2\u0083\u0084\3\2\2\2\u0084\u0082\3\2\2\2\u0084\u0085\3\2\2\2\u0085"+
		"\u0086\3\2\2\2\u0086\u0087\b\26\6\2\u0087.\3\2\2\2\u0088\u0089\7\60\2"+
		"\2\u0089\u008a\3\2\2\2\u008a\u008b\b\27\7\2\u008b\60\3\2\2\2\u008c\u008d"+
		"\7]\2\2\u008d\u008e\3\2\2\2\u008e\u008f\b\30\b\2\u008f\62\3\2\2\2\u0090"+
		"\u0091\b\31\t\2\u0091\u0092\3\2\2\2\u0092\u0093\b\31\n\2\u0093\u0094\b"+
		"\31\4\2\u0094\64\3\2\2\2\t\2\3\48k\177\u0084\13\2\3\2\7\3\2\6\2\2\7\4"+
		"\2\t\21\2\t\4\2\7\2\2\3\31\2\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}