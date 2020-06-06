lexer grammar MiScriptLexer;
WhiteSpaces: [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);
Dot: '.';
If: 'if';
End: 'end';
Then: 'then';
Else: 'else';
Var: 'var';
Is: 'is';
Equal: '=';
Do: 'do';
NotEqual: '!=';
Set: 'set';
BeginString: '"' -> pushMode(STRING_MODE);
IdentifierStart: '$';
Identifier: [a-zA-Z_\\]+;
EndExpr: ']' -> popMode;

mode STRING_MODE;
EscapeInline: '$$';
InlineExpr: '$' -> pushMode(STRING_EXPR);
EndString: '"' -> popMode;
Text: ~('"' | '$')+;

mode STRING_EXPR;
StrIdentifier: [a-zA-Z_\\]+ -> type(Identifier);
StrDot: '.'  -> type(Dot);
StartExpr: '[' -> pushMode(DEFAULT_MODE);
Other: {} -> skip, popMode;