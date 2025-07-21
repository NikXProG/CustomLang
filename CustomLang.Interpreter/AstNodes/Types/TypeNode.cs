namespace CustomLang.Interpreter.AstNodes.Types;

public class TypeNode : AstNode
{
    public TypeNode(IEnumerable<string> qualifiedName)
    {
        Namespaces = qualifiedName.ToList();

        if (Namespaces.Count > 0)
        {
            var lastItem = Namespaces.Count - 1;
            Name = Namespaces[lastItem];
            Namespaces.RemoveAt(lastItem);
        }
    }

    public TypeNode(params string[] qualifiedName)
        : this(qualifiedName.AsEnumerable())
    {
    }

    public TypeNode(TypeNode template)
    {
        Namespaces = template.Namespaces;
        Name = template.Name;
        TypeParameters = template.TypeParameters;
    }

    public override AstNodeType Kind => AstNodeType.TypeNode;

    public override void Accept(CustomLangVisitor visitor) => 
        visitor.VisitType(this);

    public List<string> Namespaces { get; set; }

    public string Name { get; set; }

    public List<TypeNode> TypeParameters { get; set; }
    
    public bool IsArray { get; set; }

    // public string AsString() =>
    //     string.Join(".", Namespaces.Concat(Enumerable.Repeat(Identifier, 1))) +
    //     (TypeParameters.IsNullOrEmpty() ? string.Empty :
    //         "<" + string.Join(", ", TypeParameters.Select(t => t.AsString())) + ">") +
    //     (IsArray ? "[]" : string.Empty);
}