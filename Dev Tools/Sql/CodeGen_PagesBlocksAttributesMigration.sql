set nocount on
declare
@crlf varchar(2) = char(13) + char(10)


begin

IF OBJECT_ID('tempdb..#codeTable') IS NOT NULL
    DROP TABLE #codeTable

create table #codeTable (
    Id int identity(1,1) not null,
    CodeText nvarchar(max),
    CONSTRAINT [pk_codeTable] PRIMARY KEY CLUSTERED  ( [Id]) );
    
	-- layouts
    insert into #codeTable
    SELECT '            AddLayout("' +
        CONVERT( nvarchar(50), [s].[Guid]) + '","'+ 
        [l].[FileName] +  '","'+
        [l].[Name] +  '","'+
        ISNULL([l].[Description],'')+  '","'+
        CONVERT( nvarchar(50), [l].[Guid])+  '");'  
    FROM [Layout] [l]
    join [Site] [s] on [s].[Id] = [l].[SiteId]
    where [l].[IsSystem] = 0
	and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site
    order by [l].[Id]

    insert into #codeTable
    SELECT @crlf

	-- pages
    insert into #codeTable
    SELECT '            AddPage("' +
        CONVERT( nvarchar(50), [pp].[Guid]) + '","'+ 
        CONVERT( nvarchar(50), [l].[Guid]) + '","'+ 
        [p].[Name]+  '","'+  
        ISNULL([p].[Description],'')+  '","'+
        CONVERT( nvarchar(50), [p].[Guid])+  '","'+  
        ISNULL([p].[IconCssClass],'')+ '");'
    FROM [Page] [p]
    join [Page] [pp] on [pp].[Id] = [p].[ParentPageId]
	join [Layout] [l] on [l].[Id] = [p].[layoutId]
	join [site] [s] on [s].[Id] = [l].[siteId]
    where [p].[IsSystem] = 0
	and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site
    order by [p].[Id]

    insert into #codeTable
    SELECT @crlf

    -- block types
    insert into #codeTable
    SELECT '            AddBlockType("'+
        [Name]+ '","'+  
        ISNULL([Description],'')+ '","'+  
        [Path]+ '","'+  
        CONVERT( nvarchar(50),[Guid])+ '");'
    from [BlockType]
    where [IsSystem] = 0
    order by [Id]

    insert into #codeTable
    SELECT @crlf

    -- blocks
    insert into #codeTable
    SELECT 
        '            // Add Block to ' + ISNULL('Page: ' + p.Name,'') + ISNULL('Layout: ' + l.Name, '') +
        @crlf + 
		'            AddBlock("'+
        ISNULL(CONVERT(nvarchar(50), [p].[Guid]),'') + '","'+ 
        ISNULL(CONVERT(nvarchar(50), [l].[Guid]),'') + '","'+
        CONVERT(nvarchar(50), [bt].[Guid])+ '","'+
        [b].[Name]+ '","'+
        [b].[Zone]+ '",'+
        CONVERT(varchar, [b].[Order])+ ',"'+
        CONVERT(nvarchar(50), [b].[Guid])+ '");'+
        @crlf
    from [Block] [b]
    join [BlockType] [bt] on [bt].[Id] = [b].[BlockTypeId]
    left outer join [Page] [p] on [p].[Id] = [b].[PageId]
	left outer join [Layout] [l] on [l].[Id] = [b].[LayoutId]
	left outer join [Layout] [pl] on [pl].[Id] = [p].[LayoutId]
	join [site] [s] on [s].[Id] = [l].[siteId] or [s].[Id] = [pl].[siteId]
    where [b].[IsSystem] = 0
	and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site
    order by [b].[Id]

    -- attributes
    if object_id('tempdb..#attributeIds') is not null
    begin
      drop table #attributeIds
    end

    select * 
	into #attributeIds 
	from (
		select A.[Id] 
		from [dbo].[Attribute] A
		inner join [EntityType] E 
			ON E.[Id] = A.[EntityTypeId]
		where A.[IsSystem] = 0
		and E.[Name] = 'Rock.Model.Block'
		and A.EntityTypeQualifierColumn = 'BlockTypeId'
	) [newattribs]
    
    insert into #codeTable
    SELECT @crlf

    insert into #codeTable
    SELECT 
        '            // Attrib for BlockType: ' + bt.Name + ':'+ a.Name+
        @crlf +
        '            AddBlockTypeAttribute("'+ 
        CONVERT(nvarchar(50), bt.Guid)+ '","'+   
        CONVERT(nvarchar(50), ft.Guid)+ '","'+     
        a.Name+ '","'+  
        a.[Key]+ '","'+ 
        ''+ '","'+ 
        --ISNULL(a.Category,'')+ '","'+ 
        ISNULL(a.Description,'')+ '",'+ 
        CONVERT(varchar, a.[Order])+ ',"'+ 
        ISNULL(a.DefaultValue,'')+ '","'+
        CONVERT(nvarchar(50), a.Guid)+ '");' +
        @crlf
    from [Attribute] [a]
    left outer join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
    left outer join [BlockType] [bt] on [bt].[Id] = cast([a].[EntityTypeQualifierValue] as int)
    where EntityTypeQualifierColumn = 'BlockTypeId'
    and [a].[id] in (select [Id] from #attributeIds)

    insert into #codeTable
    SELECT @crlf

    -- attributes values (just Block Attributes)    
    insert into #codeTable
    SELECT 
        '            // Attrib Value for Block:'+ b.Name + ', Attribute:'+ a.Name + ' ' + ISNULL('Page: ' + p.Name,'') + ISNULL('Layout: ' + l.Name, '') +
        @crlf +
        '            AddBlockAttributeValue("'+     
        CONVERT(nvarchar(50), b.Guid)+ '","'+ 
        CONVERT(nvarchar(50), a.Guid)+ '","'+ 
        ISNULL(av.Value,'')+ '");'+
        @crlf
    from [AttributeValue] [av]
    join Block b on b.Id = av.EntityId
    join Attribute a on a.id = av.AttributeId
    left outer join [Page] [p] on [p].[Id] = [b].[PageId]
	left outer join [Layout] [l] on [l].[Id] = [b].[LayoutId]
	left outer join [Layout] [pl] on [pl].[Id] = [p].[LayoutId]
	join [site] [s] on [s].[Id] = [l].[siteId] or [s].[Id] = [pl].[siteId]
    where ([av].[AttributeId] in (select [Id] from #attributeIds) or
		(b.IsSystem = 0 and a.EntityTypeQualifierColumn = 'BlockTypeId'))
	and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site

    order by b.Id
    
    drop table #attributeIds

    insert into #codeTable
    SELECT @crlf

	-- Field Types
	insert into #codeTable
	SELECT
        '            UpdateFieldType("'+    
        ft.Name+ '","'+ 
        ISNULL(ft.Description,'')+ '","'+ 
        ft.Assembly+ '","'+ 
        ft.Class+ '","'+ 
        CONVERT(nvarchar(50), ft.Guid)+ '");'+
        @crlf
    from [FieldType] [ft]
    where (ft.IsSystem = 0)

    select CodeText [MigrationUp] from #codeTable order by Id
    delete from #codeTable

    -- generate MigrationDown

    insert into #codeTable SELECT         
        '            // Attrib for BlockType: ' + bt.Name + ':'+ a.Name+
        @crlf +
        '            DeleteAttribute("'+ 
		CONVERT(nvarchar(50), [A].[Guid]) + '");' 
		from [Attribute] [A]
		inner join [EntityType] E 
			ON E.[Id] = A.[EntityTypeId]
		left outer join [BlockType] [bt] on [bt].[Id] = cast([a].[EntityTypeQualifierValue] as int)
		where A.[IsSystem] = 0
		and E.[Name] = 'Rock.Model.Block'
		and A.EntityTypeQualifierColumn = 'BlockTypeId'
		order by [A].[Id] desc   

    insert into #codeTable
    SELECT @crlf

    insert into #codeTable 
	SELECT 
        '            // Remove Block: ' + b.Name + ', from ' + ISNULL('Page: ' + p.Name,'') + ISNULL('Layout: ' + l.Name, '') +
        @crlf + 
		'            DeleteBlock("'+ CONVERT(nvarchar(50), [b].[Guid])+ '");'
        from [Block] [b]
		left outer join [Page] [p] on [p].[Id] = [b].[PageId]
		left outer join [Layout] [l] on [l].[Id] = [b].[LayoutId]
		left outer join [Layout] [pl] on [pl].[Id] = [p].[LayoutId]
		join [site] [s] on [s].[Id] = [l].[siteId] or [s].[Id] = [pl].[siteId]
		where [b].[IsSystem] = 0
		and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site
		order by [b].[Id] desc

    insert into #codeTable 
	SELECT 
		'            DeleteBlockType("'+ CONVERT(nvarchar(50), [Guid])+ '"); // '+ 
		[Name] 
	from [BlockType] 
	where [IsSystem] = 0 
	order by [Id] desc

    insert into #codeTable
    SELECT @crlf

    insert into #codeTable 
	SELECT 
		'            DeletePage("'+ CONVERT(nvarchar(50), [p].[Guid])+ '"); // '+ 
		[p].[Name]  
	from [Page] [p]
	join [Layout] [l] on [l].[Id] = [p].[layoutId]
	join [site] [s] on [s].[Id] = [l].[siteId]
    where [p].[IsSystem] = 0
	and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site
	order by [p].[Id] desc 

    insert into #codeTable
    SELECT @crlf

    insert into #codeTable
    SELECT 
	'            DeleteLayout("'+ CONVERT(nvarchar(50), [l].[Guid])+ '"); // '+ 
		[l].[Name]  
    FROM [Layout] [l]
    join [Site] [s] on [s].[Id] = [l].[SiteId]
    where [l].[IsSystem] = 0
	and [s].[Guid] <> 'F3F82256-2D66-432B-9D67-3552CD2F4C2B' -- Ignore the external site
    order by [l].[Id] desc

    select CodeText [MigrationDown] from #codeTable order by Id

IF OBJECT_ID('tempdb..#codeTable') IS NOT NULL
    DROP TABLE #codeTable

end


