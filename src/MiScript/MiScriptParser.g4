parser grammar MiScriptParser;

options { tokenVocab = MiScriptLexer; }

script
    : statement*
    ;

functionParameter
    : IdentifierStart singleIdentifier
    ;

blockStatement
    : statement*
    ;
    
statement
    : ifStatement
    | setStatement
    | callStatement
    | expressionStatement
    ;

expressionStatement
    : expression
    ;

setStatement
    : Var IdentifierStart? singleIdentifier (Is | Equal) (callStatement | expression)
    ;

callStatement
    : singleIdentifier expression*
    ;

ifStatement
    : If expression Then blockStatement elseIfStatement* elseStatement? End
    ;
    
elseIfStatement
    : Else If expression Then blockStatement
    ;

elseStatement
    : Else blockStatement
    ;
    
expression
    : IdentifierStart identifier        # IdentifierExpression
    | string                            # StringExpression
    | expression Equal expression       # EqualExpression
    | expression NotEqual expression    # NotEqualExpression
    ;

identifier
    : memberIdentifier
    | singleIdentifier
    ;
    
memberIdentifier
    : Identifier (Dot (Identifier))+
    ;
    
singleIdentifier
    : Identifier
    ;

string
    : BeginString stringPart* EndString
    ;

stringPart
    : Text
    | Dot
    | StartExpr
    | EndExpr
    | stringEscape
    | stringIdentifier
    ;

stringEscape
    : EscapeInline
    ;

stringIdentifier
    : InlineExpr StartExpr (identifier | expression) EndExpr
    | InlineExpr identifier
    ;