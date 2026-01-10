// Quill editor wrapper for Blazor interop

window.quillEditor = {
    instances: {},

    // Initialize Quill editor
    initialize: function (editorId, dotNetRef) {
        try {
            const container = document.getElementById(editorId);
            if (!container) {
                console.error(`Element with id '${editorId}' not found`);
                return false;
            }

            // Create Quill instance with toolbar
            const quill = new Quill(`#${editorId}`, {
                theme: 'snow',
                modules: {
                    toolbar: [
                        [{ 'header': [1, 2, 3, false] }],
                        ['bold', 'italic', 'underline', 'strike'],
                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                        ['blockquote', 'code-block'],
                        [{ 'color': [] }, { 'background': [] }],
                        ['link'],
                        ['clean']
                    ]
                },
                placeholder: 'Write your thoughts here...'
            });

            // Store instance
            this.instances[editorId] = quill;

            // Listen for text changes and notify Blazor
            quill.on('text-change', function () {
                const html = quill.root.innerHTML;
                dotNetRef.invokeMethodAsync('OnContentChanged', html);
            });

            console.log(`Quill editor initialized for ${editorId}`);
            return true;
        } catch (error) {
            console.error('Error initializing Quill:', error);
            return false;
        }
    },

    // Get HTML content from editor
    getContent: function (editorId) {
        const quill = this.instances[editorId];
        if (quill) {
            return quill.root.innerHTML;
        }
        return '';
    },

    // Set HTML content in editor
    setContent: function (editorId, html) {
        const quill = this.instances[editorId];
        if (quill) {
            quill.root.innerHTML = html || '';
        }
    },

    // Clear editor content
    clear: function (editorId) {
        const quill = this.instances[editorId];
        if (quill) {
            quill.setText('');
        }
    },

    // Enable/disable editor
    enable: function (editorId, enabled) {
        const quill = this.instances[editorId];
        if (quill) {
            quill.enable(enabled);
        }
    },

    // Destroy editor instance
    destroy: function (editorId) {
        const quill = this.instances[editorId];
        if (quill) {
            delete this.instances[editorId];
            return true;
        }
        return false;
    }
};