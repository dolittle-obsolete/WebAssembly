config = {
 	vfs_prefix: "managed",
 	deploy_prefix: "managed",
 	enable_debugging: 0,
 	file_list: [  ],
	add_bindings: function() { Module.'mono'_bindings_init ("[WebAssembly.Bindings]WebAssembly.Runtime"); }
}
