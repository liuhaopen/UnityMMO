local UnsafeLinkedListNode = ECS.BaseClass()
ECS.UnsafeLinkedListNode = UnsafeLinkedListNode

function UnsafeLinkedListNode:Constructor(  )
end

function UnsafeLinkedListNode.InitializeList( list )
	list.Prev = list
    list.Next = list
end

function UnsafeLinkedListNode:Begin(  )
    return self.Next
end

function UnsafeLinkedListNode:IsEmpty(  )
	return self == self.Next
end

function UnsafeLinkedListNode:GetChunk( )
    return self.chunk
end

function UnsafeLinkedListNode:SetChunk( value )
    self.chunk = value
end

function UnsafeLinkedListNode:Add( node )
    UnsafeLinkedListNode.InsertBefore(self, node)
end

function UnsafeLinkedListNode:Remove(  )
	if (self.Prev == nil) then
        return
    end

    self.Prev.Next = self.Next
    self.Next.Prev = self.Prev
    self.Prev = nil
    self.Next = nil
end

function UnsafeLinkedListNode.InsertBefore( pos, node )
	assert(node ~= pos, "cannot be same!")
    -- Assert.IsFalse(node.IsInList)
    node.Prev = pos.Prev
    node.Next = pos

    node.Prev.Next = node
    node.Next.Prev = node
end

return UnsafeLinkedListNode