local UnsafeLinkedListNode = BaseClass()
ECS.UnsafeLinkedListNode = UnsafeLinkedListNode

function UnsafeLinkedListNode:Constructor(  )
	
end

function UnsafeLinkedListNode.InitializeList( list )
	list.Prev = list
    list.Next = list
end

return UnsafeLinkedListNode