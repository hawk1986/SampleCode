/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

	config.htmlEncodeOutput = true;
	config.filebrowserImageUploadUrl = '/Home/UploadPicture';
	config.height = 300;
	config.image_previewText = ' ';
	//config.fontSize_sizes = '12/1.2rem;13/1.3rem;14/1.4rem;15/1.5rem;16/1.6rem;17/1.7rem;18/1.8rem;19/1.9rem;20/2.0rem;21/2.1rem;24/2.4rem;26/2.6rem;28/2.8rem;30/3.0rem;36/3.6rem';

	// Dialog windows are also simplified.
	config.removeDialogTabs = 'link:advanced';


	config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		'/',
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		'/',
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
	];

	config.removeButtons = 'Save,NewPage,Preview,Print,Templates,Undo,Redo,Find,Replace,Checkbox,TextField,Textarea,Radio,Select,Button,ImageButton,HiddenField,Subscript,Superscript,CopyFormatting,RemoveFormat,NumberedList,BulletedList,CreateDiv,Language,Anchor,Flash,Iframe,Styles,Format,About';
};